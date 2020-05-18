using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class SquadQueryComponent : BaseComponent
    {
        

        private readonly FalconDbContext _dbContext;
        public SquadQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;            
        }

        /// <summary>
        /// Get Squad by id
        /// </summary>
        /// <param name="key">Squad Id</param>
        /// <returns></returns>
        public async Task<SquadGetRp> GetSquadById(int id)
        {
            var entity = await this._dbContext.Squads
                .Include(c=>c.Members).ThenInclude(c=>c.User)
                .Include(c => c.FeatureMaps).ThenInclude(c => c.Feature)
                                         .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(c=>c.Id == id);

            if (entity == null)
                return null;

            return this._mapper.Map<SquadGetRp>(entity);
        }

        public async Task<SquadGetDetailRp> GetSquadByIdWithAvailability(int id, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Squads                
                .Include(c => c.Members).ThenInclude(c => c.User)
                .Include(c => c.FeatureMaps)
                        .ThenInclude(c => c.Feature)
                        .ThenInclude(c => c.ServiceMaps)                                             
                .SingleOrDefaultAsync(c => c.Id == id );

            var validProducts = entity.FeatureMaps.Select(c => c.Feature.ProductId).Distinct();

            var products = await this._dbContext.Products.Where(c => validProducts.Contains(c.Id.Value)).ToListAsync();

            var validServices = entity.FeatureMaps
                .SelectMany(c => c.Feature.ServiceMaps)
                .Select(c => c.ServiceId).Distinct();

            var services = await this._dbContext.Services
                .Include(c => c.FeatureMap)
                .ThenInclude(c => c.Feature)
                .ThenInclude(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .Where(c => validServices.Contains(c.Id.Value)).ToListAsync();
                        
            var productIds = entity.FeatureMaps.Select(c => c.Feature.ProductId).Distinct().ToList();

            var sourceItems =await  this._dbContext.GetSourceItemsByProduct(productIds, start, end);

            foreach (var service in services)
            {
                service.Product = products.Single(c => c.Id == service.ProductId);
                foreach (var feature in service.FeatureMap)
                {
                    feature.Feature.Product = products.Single(c => c.Id == feature.Feature.ProductId);
                    foreach (var indicator in feature.Feature.Indicators)
                    {
                        indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                    }
                }                
            }
                        
            var agg = new SquadPointsAggregate(entity);
            var squadPonts = agg.MeasurePoints();

            SquadGetDetailRp result = this._mapper.Map<SquadGetDetailRp>(entity);

            foreach (var item in squadPonts)
            {
                
                var tmp = new FeatureBySquadRp()
                {
                    Id = item.feature.Id.Value,
                    Description = item.feature.Description,
                    Avatar = item.feature.Avatar,
                    CreatedBy = item.feature.CreatedBy,
                    CreatedOn = item.feature.CreatedOn,
                    Quality = item.quality,
                    ProductId = item.product.Id.Value,
                    Product = item.product.Name,
                    ServiceId = item.service.Id.Value,
                    ServiceAvatar = item.service.Avatar,                    
                };
                tmp.Name = item.feature.Name;
                tmp.SLO = item.service.AvailabilitySlo;
                tmp.Service = item.service.Name;
                tmp.Impact = QualityUtils.MeasureImpact(item.service.AvailabilitySlo);
                tmp.Points = item.points;

                result.Points += tmp.Points;
                result.Features.Add(tmp);
            }

            result.Features = result.Features.OrderBy(c => c.Service).ToList();
            return result;
        }

        public async Task<SquadGetRp> GetSquadByName(int customerId, string name)
        {
            var entity = await this._dbContext.Squads.SingleOrDefaultAsync(c => c.Customer.Id == customerId && c.Name == name );

            return this._mapper.Map<SquadGetRp>(entity);
        }

        /// <summary>
        /// Get All Squad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SquadGetListRp>> GetSquads(int customerId)
        {
            var entities = await this._dbContext.Squads
                .Include(c=>c.Members)
                .Where(c=> c.Customer.Id.Equals(customerId)).ToListAsync();

            return this._mapper.Map<IEnumerable<SquadGetListRp>>(entities);
        }

        
        public async Task<IEnumerable<SquadGetListRp>> GetSquadsWithPoints(int customerId, 
            DateTime start, 
            DateTime end)
        {
            var entities = await this._dbContext.Squads
                .Include(c => c.Members)
                .Where(c => c.Customer.Id.Equals(customerId)).ToListAsync();
            List<SquadGetListRp> result = new List<SquadGetListRp>();
            foreach (var squad in entities)
            {
                var tmp = await this.GetSquadByIdWithAvailability(squad.Id.Value, start, end);
                
                result.Add(new SquadGetListRp() {
                     Id = tmp.Id,
                     Name = tmp.Name,
                     Avatar = tmp.Avatar,
                     Points = tmp.Points,
                     Features = tmp.Features.Count(),
                     Members = squad.Members.Count()
                });
            }
            return result;
        }


    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Aggregates;
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
        private readonly FeatureQueryComponent _featureQueryComponent;

        private readonly FalconDbContext _dbContext;
        public SquadQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService,
            FeatureQueryComponent featureQueryComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            this._featureQueryComponent = featureQueryComponent;
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
                .Include(c => c.Features).ThenInclude(c => c.Feature)
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
                .Include(c => c.Features)
                        .ThenInclude(c => c.Feature)
                        .ThenInclude(c => c.ServiceMaps)
                        .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(c => c.Id == id);

            var productIds = entity.Features.Select(c => c.Feature.ProductId).Distinct().ToList();

            var sourceItems = this._dbContext.GetSourceItemsByProduct(productIds, start, end);

            foreach (var service in entity.Services)
            {
                foreach (var indicator in service.Feature.Indicators)
                {
                    indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
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
                    Availability = item.points,
                    ProductId = item.product.Id.Value,
                    Product = item.product.Name,
                    ServiceId = item.service.Id.Value,
                    ServiceAvatar = item.service.Avatar,
                    Service =  item.service.Name,
                    SLO = item.service.Slo,
                    Name = item.feature.Name,
                    Impact = AvailabilityUtils.MeasureImpact(item.service.Slo),
                    Points = AvailabilityUtils.MeasurePoints(item.service.Slo, item.points)
                };

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
                .Where(c=> c.Customer.Id.Equals(customerId)).ToListAsync();

            return this._mapper.Map<IEnumerable<SquadGetListRp>>(entities);
        }

        
        public async Task<IEnumerable<SquadGetListRp>> GetSquadsWithPoints(int customerId, 
            DateTime start, 
            DateTime end)
        {
            var entities = await this._dbContext.Squads
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
                     Features = tmp.Features.Count()
                });
            }
            return result;
        }


    }
}

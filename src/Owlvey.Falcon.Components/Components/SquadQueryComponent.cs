using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Owlvey.Falcon.Repositories.Products;
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
            IUserIdentityGateway identityGateway, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
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

        public async Task<SquadQualityGetRp> GetSquadByIdWithQuality(int id, DatePeriodValue period)
        {
            var entity = await this._dbContext.Squads                
                .Include(c => c.Members).ThenInclude(c => c.User)
                .Include(c => c.FeatureMaps).ThenInclude(c=>c.Feature)            
                .SingleOrDefaultAsync(c => c.Id == id );

            var members = await this._dbContext.Members.Include(c=>c.User)
                .Where(c=>c.SquadId == id)
                .ToListAsync();
            var validProducts = entity.FeatureMaps.Select(c => c.Feature.ProductId).Distinct();

            var products = new List<ProductEntity>();

            foreach (var item in validProducts)
            {
                var tmp = await this._dbContext.FullLoadProductWithSourceItems(item, period.Start, period.End);
                products.Add(tmp);
            }
            foreach (var item in entity.FeatureMaps)
            {
                foreach (var product in products)
                {
                    var tmp =  product.Features.Where(c => c.Id == item.FeatureId).SingleOrDefault();
                    if (tmp != null)
                    {
                        item.Feature = tmp;
                        break;
                    }
                }
            }
                                    
            var agg = new SquadPointsAggregate(entity);
            var squadMeasures = agg.Measure();

            var  result = this._mapper.Map<SquadQualityGetRp>(entity);
                        
            result.Members = this._mapper.Map<IEnumerable<UserGetListRp>>(members.Select(c => c.User).Distinct(new UserEntityCompare()));

            foreach (var item in squadMeasures)
            {                
                var tmp = new FeatureBySquadRp()
                {
                    Id = item.feature.Id.Value,
                    Description = item.feature.Description,
                    Avatar = item.feature.Avatar,
                    CreatedBy = item.feature.CreatedBy,
                    CreatedOn = item.feature.CreatedOn,
                    Debt = item.debt,
                    Quality = item.quality,
                    ProductId = item.product.Id.Value,
                    Product = item.product.Name,
                    JourneyId = item.journey.Id.Value,
                    JourneyAvatar = item.journey.Avatar,                    
                };
                tmp.Name = item.feature.Name;
                tmp.SLO = item.journey.GetSLO();
                tmp.Journey = item.journey.Name;                
                result.Features.Add(tmp);
            }
            result.Features = result.Features.OrderBy(c => c.Journey).ToList();
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
            DatePeriodValue period)
        {            
            var entities = await this._dbContext.Squads
                .Include(c => c.Members)
                .Where(c => c.Customer.Id.Equals(customerId)).ToListAsync();
            List<SquadGetListRp> result = new List<SquadGetListRp>();
            foreach (var squad in entities)
            {
                var tmp = await this.GetSquadByIdWithQuality(squad.Id.Value,  period);
                
                result.Add(new SquadGetListRp() {
                     Id = tmp.Id,
                     Name = tmp.Name,
                     Avatar = tmp.Avatar,
                     Debt = tmp.Debt,
                     Features = tmp.Features.Select(c=>c.Name).Distinct().Count(),
                     Members = squad.Members.Count()
                });
            }

            return result;
        }


    }
}

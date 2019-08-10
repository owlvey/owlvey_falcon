using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
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
    public class FeatureQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        
        public FeatureQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper, IUserIdentityGateway userIdentityGateway) : base(dateTimeGateway, mapper, userIdentityGateway)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get Feature by id
        /// </summary>
        /// <param name="key">Feature Id</param>
        /// <returns></returns>
        public async Task<FeatureGetRp> GetFeatureById(int id)
        {
            var entity = await this._dbContext.Features.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return this._mapper.Map<FeatureGetRp>(entity);
        }


        public async Task<FeatureGetRp> GetFeatureByName(int productId, string name)
        {
            var entity = await this._dbContext.Features.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);

            if (entity == null)
                return null;

            return this._mapper.Map<FeatureGetRp>(entity);
        }

        /// <summary>
        /// Get All Feature
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FeatureGetListRp>> GetFeatures(int productId)
        {
            var entities = await this._dbContext.Features.Where(c=> c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable< FeatureGetListRp>>(entities);
        }

        public async Task<SeriesGetRp> GetDailySeriesById(int featureId, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Features.Include(c=>c.Indicators.Select(d=>d.Source)).SingleAsync(c => c.Id == featureId);

            foreach (var indicator in entity.Indicators)
            {
                var sourceItems = await this._dbContext.SourcesItems.Where(c => c.SourceId == indicator.Source.Id && c.Start >= start && c.End <= end).ToListAsync();
                indicator.Source.SourceItems = sourceItems;
            }

            var result = new SeriesGetRp
            {
                Start = start,
                End = end,
                Name = entity.Name,
                Avatar = entity.Avatar
            };

            var aggregator = new FeatureAvailabilityAggregate(entity, start, end);

            var (_, items) = aggregator.MeasureAvailability();
            
            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));
            }

            return result;
        }
    }
}

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
            var entities = await this._dbContext.Features.Include(c=>c.Indicators).Where(c=> c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable< FeatureGetListRp>>(entities);
        }


        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesBy(int productId)
        {
            var entities = await this._dbContext.Features.Where(c => c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(entities);
        }

        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesByServiceId(int serviceId) {
            var entities = await this._dbContext.ServiceMaps.Include(c=>c.Feature).Where(c => c.Service.Id == serviceId).ToListAsync();
            var features = entities.Select(c => c.Feature).ToList();
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(features);
        }

        public async Task<MultiSeriesGetRp> GetDailySeriesById(int featureId, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Features.Include(c=>c.Indicators).ThenInclude(c=>c.Source).SingleAsync(c => c.Id == featureId);

            foreach (var indicator in entity.Indicators)
            {
                var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                indicator.Source.SourceItems = sourceItems;
            }

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = entity.Name,
                Avatar = entity.Avatar
            };

            var aggregator = new FeatureAvailabilityAggregate(entity, start, end);

            var (feature, availability, indicators) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                 Name = "Availability",
                 Avatar = entity.Avatar,
                 Items = availability.Select(c=> this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in indicators)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("SLI:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }

            return result;
        }
    }
}

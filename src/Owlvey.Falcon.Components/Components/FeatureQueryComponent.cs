using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Repositories.Features;
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
using Owlvey.Falcon.Repositories.Services;

namespace Owlvey.Falcon.Components
{
    public class FeatureQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public FeatureQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway,
            IMapper mapper, IUserIdentityGateway userIdentityGateway,
            ConfigurationComponent configuration) : base(dateTimeGateway, mapper, userIdentityGateway, configuration)
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
            var entity = await this._dbContext.Features
                .Include(c => c.Indicators).ThenInclude(c=>c.Source)
                .Include(c =>c.Squads).ThenInclude(c=>c.Squad)
                .Where(c => c.Id.Equals(id))
                .SingleOrDefaultAsync();

            if (entity == null) {
                return null;
            }

            var result = this._mapper.Map<FeatureGetRp>(entity);

            foreach (var item in entity.Indicators)
            {
                result.Indicators.Add(new IndicatorGetListRp() {
                     Id = item.Id.Value,
                     FeatureId = item.FeatureId,
                     SourceId = item.SourceId,
                     CreatedBy = item.CreatedBy,
                     CreatedOn = item.CreatedOn,
                     Source = item.Source.Name
                });
            }
            result.Indicators = result.Indicators.OrderBy(c => c.Id).ToList();
            return result;
        }

        public async Task<IEnumerable<FeatureGetListRp>> SearchFeatureByName(int productId, string name) {
            var entities = await this._dbContext.Features.Where(c => c.ProductId == productId && c.Name.Contains(name)).ToListAsync();
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(entities);
        }

        public async Task<FeatureQualityGetRp> GetFeatureByIdWithAvailability(int id, DateTime start, DateTime end)
        {
            var feature = await this._dbContext.Features
                .Include(c => c.Squads).ThenInclude(c => c.Squad)
                .Include(c => c.Indicators).ThenInclude(c => c.Source)
                .Include(c => c.ServiceMaps).ThenInclude(c => c.Service)
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();


            if (feature == null)
            {
                return null;
            }

            foreach (var indicator in feature.Indicators)
            {
                var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                indicator.Source.SourceItems = sourceItems;
            }

            var agg = new FeatureAvailabilityAggregate(feature);

            var model = this._mapper.Map<FeatureQualityGetRp>(feature);

            (model.Quality, _, _, model.Availability, model.Latency) = agg.MeasureQuality();

            foreach (var indicator in feature.Indicators)
            {                
                var tmp = this._mapper.Map<IndicatorAvailabilityGetListRp>(indicator);
                (tmp.Availability, tmp.Total, tmp.Good) = (new IndicatorDateAvailabilityAggregate(indicator)).MeasureAvailability();                
                model.Indicators.Add(tmp);
            }

            model.Indicators = model.Indicators.OrderByDescending(c => c.Availability).ToList();

            foreach (var map in feature.ServiceMaps)
            {
                var tmp = this._mapper.Map<ServiceGetListRp>(map.Service);
                model.Services.Add(tmp);
            }

            model.Incidents = this._mapper.Map<List<IncidentGetListRp>>(await this._dbContext.GetIncidentsByFeature(feature.Id.Value));

            if (model.Incidents.Count() > 0)
            {
                model.MTTM = DateTimeUtils.FormatTimeToInMinutes(model.Incidents.Average(c => c.TTM));
                model.MTTE = DateTimeUtils.FormatTimeToInMinutes(model.Incidents.Average(c => c.TTE));
                model.MTTD = DateTimeUtils.FormatTimeToInMinutes(model.Incidents.Average(c => c.TTD));
                model.MTTF = DateTimeUtils.FormatTimeToInMinutes(model.Incidents.Average(c => c.TTF));
            }
            return model;
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
            var entities = await this._dbContext.Features.Include(c => c.Indicators).Where(c => c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(entities);
        }

        public async Task<IEnumerable<FeatureAvailabilityGetListRp>> GetFeaturesWithQuality(int productId,
            DateTime start, DateTime end)
        {
            var result = new List<FeatureAvailabilityGetListRp>();

            var entities = await this._dbContext.Features
                .Include(c => c.ServiceMaps)
                .Include(c => c.IncidentMap).ThenInclude(c => c.Incident)
                .Include(c => c.Indicators).ThenInclude(c => c.Source)
                .Include(c=> c.Squads)
                .Where(c => c.Product.Id.Value.Equals(productId)).ToListAsync();

            var sources = entities.SelectMany(c => c.Indicators).Select(c => c.SourceId).Distinct().ToList();
            var sourceItems = await this._dbContext.GetSourceItems(sources, start, end);

            foreach (var feature in entities)
            {
                foreach (var indicator in feature.Indicators)
                {                    
                    indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                }
                var tmp = this._mapper.Map<FeatureAvailabilityGetListRp>(feature);
                var agg = new FeatureAvailabilityAggregate(feature);                
                (tmp.Quality, _, _, tmp.Availability, tmp.Latency) = agg.MeasureQuality();                                
                tmp.Squads = feature.Squads.Count();                
                tmp.Total = feature.Indicators.Sum(c => c.Source.SourceItems.Sum(d => d.Total));
                tmp.Good = feature.Indicators.Sum(c => c.Source.SourceItems.Sum(d => d.Good));
                tmp.ServiceCount = feature.ServiceMaps.Count();                
                result.Add(tmp);
            }
            return result.OrderBy(c => c.Availability).ToList();
        }


        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesBy(int productId)
        {
            var entities = await this._dbContext.Features.Where(c => c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(entities);
        }

        public async Task<ICollection<FeatureGetListRp>> GetFeaturesByServiceId(int serviceId)
        {
            var entities = await this._dbContext.ServiceMaps.Include(c => c.Feature).Where(c => c.Service.Id == serviceId).ToListAsync();
            var features = entities.Select(c => c.Feature).ToList();
            return this._mapper.Map<ICollection<FeatureGetListRp>>(features);
        }

        public async Task<ICollection<SequenceFeatureGetListRp>> GetFeaturesByServiceIdWithAvailability(int serviceId, DateTime start, DateTime end)
        {
            var service = await this._dbContext.GetService(serviceId);
            
            var result = new List<SequenceFeatureGetListRp>();                                   

            var sources = service.FeatureMap.SelectMany(c => c.Feature.Indicators).Select(c => c.SourceId).Distinct().ToList();
            var sourceItems = await this._dbContext.GetSourceItems(sources, start, end);

            foreach (var map in service.FeatureMap)            
            {
                var feature = map.Feature;

                foreach (var indicator in feature.Indicators)
                {                    
                    indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                }

                var tmp = this._mapper.Map<SequenceFeatureGetListRp>(feature);
                tmp.FeatureSlo = service.FeatureSLO;
                var agg = new FeatureAvailabilityAggregate(feature);
                (tmp.Quality, _, _, tmp.Availability, tmp.Latency) = agg.MeasureQuality();                
                tmp.Total = feature.Indicators.Sum(c => c.Source.SourceItems.Sum(d => d.Total));
                tmp.MapId = map.Id.Value;                
                result.Add(tmp);
            }

            int sequence = 0;
            result = result.OrderBy(c => c.MapId).ToList();
            foreach (var item in result)
            {
                item.Sequence = sequence;
                sequence += 1;
            }
            return result;
        }

        public async Task<MultiSeriesGetRp> GetDailySeriesById(int featureId, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Features.Include(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .SingleAsync(c => c.Id == featureId);

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

            var aggregator = new FeatureDailyAvailabilityAggregate(entity, start, end);

            var (feature, availability, indicators) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = entity.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
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

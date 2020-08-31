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
using Owlvey.Falcon.Repositories.Journeys;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Repositories.Products;

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

        public async Task<FeatureQualityGetRp> GetFeatureByIdWithQuality(int id, 
            DatePeriodValue period)
        {
            var feature = await this._dbContext.Features
                .Include(c => c.Squads).ThenInclude(c => c.Squad)
                .Include(c => c.Indicators).ThenInclude(c => c.Source)
                .Include(c => c.JourneyMaps).ThenInclude(c => c.Journey)
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();


            if (feature == null)
            {
                return null;
            }

            foreach (var indicator in feature.Indicators)
            {
                var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, period.Start, period.End);
                indicator.Source.SourceItems = sourceItems;
            }
                        
            var model = this._mapper.Map<FeatureQualityGetRp>(feature);
            var measure = feature.Measure();
            model.LoadQuality(measure);
            model.Debt = feature.MeasureDebt();
            foreach (var indicator in feature.Indicators)
            {                
                var tmp = this._mapper.Map<IndicatorDetailRp>(indicator);                
                tmp.Measure = indicator.Source.Measure();                
                model.Indicators.Add(tmp);
            }            

            foreach (var map in feature.JourneyMaps)
            {
                var tmp = this._mapper.Map<JourneyGetListRp>(map.Journey);
                model.Journeys.Add(tmp);
            }

            model.Incidents = this._mapper.Map<List<IncidentGetListRp>>(await this._dbContext.GetIncidentsByFeature(feature.Id.Value));
                        
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
            DatePeriodValue period)
        {
            var result = new List<FeatureAvailabilityGetListRp>();

            var product =  await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);                                   

            foreach (var feature in product.Features)
            { 
                var tmp = this._mapper.Map<FeatureAvailabilityGetListRp>(feature);
                tmp.Quality  = feature.Measure();                                                 
                tmp.Squads = feature.Squads.Count();                                
                tmp.JourneyCount = feature.JourneyMaps.Count();
                tmp.Debt = feature.MeasureDebt();                
                result.Add(tmp);
            }
            return result.OrderBy(c => c.Quality.Availability).ToList();
        }


        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesBy(int productId)
        {
            var entities = await this._dbContext.Features.Where(c => c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(entities);
        }

        public async Task<ICollection<FeatureGetListRp>> GetFeaturesByJourneyId(int journeyId)
        {
            var entities = await this._dbContext.JourneyMaps.Include(c => c.Feature).Where(c => c.Journey.Id == journeyId).ToListAsync();
            var features = entities.Select(c => c.Feature).ToList();
            return this._mapper.Map<ICollection<FeatureGetListRp>>(features);
        }

        public async Task<ICollection<SequenceFeatureGetListRp>> GetFeaturesByJourneyIdWithAvailability(int journeyId, DateTime start, DateTime end)
        {
            var journey = await this._dbContext.GetJourney(journeyId);
            
            var result = new List<SequenceFeatureGetListRp>();                                   

            var sources = journey.FeatureMap.SelectMany(c => c.Feature.Indicators).Select(c => c.SourceId).Distinct().ToList();
            var sourceItems = await this._dbContext.GetSourceItems(sources, start, end);

            foreach (var map in journey.FeatureMap)            
            {
                var feature = map.Feature;

                foreach (var indicator in feature.Indicators)
                {                    
                    indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                }

                var tmp = this._mapper.Map<SequenceFeatureGetListRp>(feature);                                
                var measure = feature.Measure();                
                tmp.Availability = measure.Availability;
                tmp.Experience = measure.Experience;
                tmp.Latency = measure.Latency;                
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

        public async Task<MultiSeriesGetRp> GetDailyAvailabilitySeriesById(int featureId,
            DateTime start, DateTime end)
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

            var aggregator = new FeatureDailyAvailabilityAggregate(entity, new DatePeriodValue( start, end));

            var (availability, indicators) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = entity.Avatar,
                Items = availability.Select(c => new SeriesItemGetRp(c.Date, c.Measure.Availability)).ToList()
            }); 

            foreach (var indicator in indicators)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("SLI:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => new SeriesItemGetRp(c.Date, c.Measure.Availability)).ToList()
                });
            }

            return result;
        }
    }
}

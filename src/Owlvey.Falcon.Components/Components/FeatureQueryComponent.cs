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

        public async Task<FeatureGetRp> GetFeatureByIdWithAvailability(int id, DateTime start, DateTime end)
        {
            var feature = await this._dbContext.Features.Where(c => c.Id == id)
                .Include(c => c.Squads).ThenInclude(c => c.Squad)
                .Include(c => c.Indicators).ThenInclude(c => c.Source)
                .Include(c => c.ServiceMaps).ThenInclude(c => c.Service)
                .FirstOrDefaultAsync();

            if ( feature == null  ) {
                return null;
            }

            foreach (var indicator in feature.Indicators)
            {
                var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                indicator.Source.SourceItems = sourceItems;
            }

            var agg = new FeatureDateAvailabilityAggregate(feature);
                       
            var model = this._mapper.Map<FeatureGetRp>(feature);
            
            model.Availability = agg.MeasureAvailability();

            foreach (var indicator in feature.Indicators)
            {                
                var tmp = this._mapper.Map<IndicatorGetListRp>(indicator);
                tmp.Availability = (new IndicatorDateAvailabilityAggregate(indicator)).MeasureAvailability();
                model.Indicators.Add(tmp);
            }

            model.Indicators = model.Indicators.OrderByDescending(c => c.Availability).ToList();

            foreach (var map in  feature.ServiceMaps)
            {
                var tmp = this._mapper.Map<ServiceGetListRp>(map.Service);
                model.Services.Add(tmp);
            }
            
            model.Incidents = this._mapper.Map<List<IncidentGetListRp>>(await this._dbContext.GetIncidentsByFeature(id, start, end));

            if (model.Incidents.Count() > 0) {
                model.MTTM = (int)model.Incidents.Average(c => c.TTM);
                model.MTTE = (int)model.Incidents.Average(c => c.TTE);
                model.MTTD = (int)model.Incidents.Average(c => c.TTD);
                model.MTTF = (int)model.Incidents.Average(c => c.TTF);
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
            var entities = await this._dbContext.Features.Include(c=>c.Indicators).Where(c=> c.Product.Id.Value.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable< FeatureGetListRp>>(entities);
        }

        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesByFilter(int productId, string filter) {

            IEnumerable<FeatureGetListRp> result = new List<FeatureGetListRp>();

            var queries = this._dbContext.Features
                .Include(c=>c.ServiceMaps)
                .Include(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .Where(c => c.Product.Id.Value.Equals(productId));

            var (field, ope, value) = FilterUtils.ParseQuery(filter);

            if (field == "serviceId" && ope == FilterOperator.ne) {

                var features  = await this._dbContext.Features.Where(c => c.ProductId == productId).ToListAsync();
                var service_feature = await this._dbContext.Services
                    .Include(c => c.FeatureMap)
                    .Where(c => c.Id == int.Parse(value))
                    .ToListAsync();

                var registered_features = service_feature.SelectMany(c => c.FeatureMap).Select(c => c.Feature).ToList();
                var complement = features.Except( registered_features , new FeatureCompare());
                result = this._mapper.Map<IEnumerable<FeatureGetListRp>>(complement);
            }

            return result;

        }

        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesWithAvailability(int productId, DateTime start, DateTime end)
        {
            var result = new List<FeatureGetListRp>();

            var entities = await this._dbContext.Features
                .Include(c => c.ServiceMaps)
                .Include(c => c.IncidentMap).ThenInclude(c=> c.Incident)
                .Include(c => c.Indicators).ThenInclude(c=>c.Source)
                .Where(c => c.Product.Id.Value.Equals(productId)).ToListAsync();
                        
            var common = new FeatureCommonComponent(this._dbContext, this._datetimeGateway); 
            foreach (var feature in entities)
            {
                var tmp = this._mapper.Map<FeatureGetListRp>(feature);

                tmp.Availability = await common.GetAvailabilityByFeature(feature, start, end);
                tmp.ServiceCount = feature.ServiceMaps.Count();
                var tmpIncidents = await this._dbContext.GetIncidentsByFeature(feature.Id.Value, start, end);
                if (tmpIncidents.Count() > 0)
                {
                    tmp.MTTM = (int)tmpIncidents.Average(c => c.TTM);
                    tmp.MTTD = (int)tmpIncidents.Average(c => c.TTD);
                    tmp.MTTE = (int)tmpIncidents.Average(c => c.TTE);
                    tmp.MTTF = (int)tmpIncidents.Average(c => c.TTF);
                }                
                result.Add(tmp);
            }
            return result.OrderBy(c=>c.Availability).ToList();
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

        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesByServiceIdWithAvailability(int serviceId, DateTime start, DateTime end)
        {
            var entities = await this._dbContext.ServiceMaps.Include(c => c.Feature).ThenInclude(c=>c.Indicators).ThenInclude(c=>c.Source).Where(c => c.Service.Id == serviceId).ToListAsync();
            var result = new List<FeatureGetListRp>();

            var common = new FeatureCommonComponent(this._dbContext, this._datetimeGateway);

            foreach (var feature in entities.Select(c=>c.Feature))
            {
                var tmp = this._mapper.Map<FeatureGetListRp>(feature);
                tmp.Availability = await common.GetAvailabilityByFeature(feature, start, end);
                var incidents = await this._dbContext.GetIncidentsByFeature(feature.Id.Value, start, end);
                if (incidents.Count() > 0) {
                    var (mttd, mtte, mttf, mttm) = (new IncidentMetricAggregate(incidents)).Metrics();
                    tmp.MTTD = mttd;
                    tmp.MTTE = mtte;
                    tmp.MTTF = mttf;
                    tmp.MTTM = mttm;
                }
                result.Add(tmp);
            }
            return result.OrderBy(c => c.Availability).ToList();            
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

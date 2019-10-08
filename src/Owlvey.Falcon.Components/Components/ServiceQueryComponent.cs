using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Owlvey.Falcon.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class ServiceQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly CacheComponent _cacheComponent;

        public ServiceQueryComponent(FalconDbContext dbContext,
            IDateTimeGateway dateTimeGateway,
            IMapper mapper,
            IUserIdentityGateway identityService,
            CacheComponent cacheComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            this._cacheComponent = cacheComponent;
        }

        public async Task<IEnumerable<ServiceGetListRp>> GetServicesWithAvailability(int productId, DateTime start, DateTime end)
        {

            var entity = await this._dbContext.Products
                .Include(c => c.Services)
                .ThenInclude(c => c.FeatureMap)
                .ThenInclude(c => c.Feature)
                .ThenInclude(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .Where(c => c.Id.Equals(productId))
                .FirstOrDefaultAsync();

            var result = new List<ServiceGetListRp>();
            foreach (var item in entity.Services)
            {

                var tmp = this._mapper.Map<ServiceGetListRp>(item);
                tmp.Availability = await this.MeasureAvailability(item, start, end);
                var incidents = await this._dbContext.GetIncidentsByService(item.Id.Value);

                if (incidents.Count() > 0)
                {
                    var agg = new IncidentMetricAggregate(incidents);
                    var (mttd, mtte, mttf, mttm) = agg.Metrics();
                    tmp.MTTD = mttd;
                    tmp.MTTE = mtte;
                    tmp.MTTF = mttf;
                }

                tmp.BudgetMinutes = AvailabilityUtils.MeasureBudgetInMinutes(tmp.Budget, start, end);

                tmp.Risk = "low";

                if (tmp.Budget > 0)
                {
                    tmp.Deploy = "innovate";
                    if (tmp.BudgetMinutes < tmp.MTTM)
                    {
                        tmp.Risk = "high";
                    }
                }
                else
                {
                    tmp.Deploy = "improve";
                    tmp.Risk = "high";
                }
                result.Add(tmp);
            }

            return result;
        }

        public async Task<ServiceGetRp> GetServiceById(int id)
        {
            var entity = await this._dbContext.Services.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (entity == null)
                return null;

            return this._mapper.Map<ServiceGetRp>(entity);
        }

        public async Task<ServiceGetRp> GetServiceByIdWithAvailabilities(int id, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Services.Include(c => c.FeatureMap)
                .ThenInclude(c => c.Feature)
                .ThenInclude(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .Where(c => c.Id == id).FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var featuresIds = entity.FeatureMap.Select(c => c.FeatureId).Distinct().ToList();
            var incidents = await this._dbContext.GetIncidentsByService(id);

            var model = this._mapper.Map<ServiceGetRp>(entity);
            model.MTTD = AvailabilityUtils.MeanTimeInMinutes(incidents.Select(c => c.TTD));
            model.MTTE = AvailabilityUtils.MeanTimeInMinutes(incidents.Select(c => c.TTE));
            model.MTTF = AvailabilityUtils.MeanTimeInMinutes(incidents.Select(c => c.TTF));
            model.Availability = await this.MeasureAvailability(entity, start, end);
            return model;
        }
        private async Task<decimal> MeasureAvailability(ServiceEntity entity, DateTime start, DateTime end)
        {
            foreach (var map in entity.FeatureMap)
            {
                foreach (var indicator in map.Feature.Indicators)
                {
                    var sourceItems = this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                    indicator.Source.SourceItems = sourceItems;
                }
            }
            var agg = new ServiceAvailabilityAggregate(entity);
            return agg.MeasureAvailability();
        }
        public async Task<ServiceGetRp> GetServiceByName(int productId, string name)
        {
            var entity = await this._dbContext.Services.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);
            return this._mapper.Map<ServiceGetRp>(entity);
        }

        public async Task<IEnumerable<ServiceGetListRp>> GetServices(int productId)
        {
            var entities = await this._dbContext.Services.Include(c => c.FeatureMap).ThenInclude(c => c.Feature).Where(c => c.Product.Id.Equals(productId)).ToListAsync();

            return this._mapper.Map<IEnumerable<ServiceGetListRp>>(entities);
        }

        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesComplement(int serviceId)
        {
            var service = await this._dbContext.Services.Include(c => c.FeatureMap).ThenInclude(c => c.Feature).Where(c => c.Id == serviceId).SingleAsync();

            var serviceFeatures = service.FeatureMap.Select(c => c.Feature).ToList();
            var features = await this._dbContext.Features.Where(c => c.ProductId == service.ProductId).ToListAsync();
            var rest = features.Except(serviceFeatures, new FeatureEntityCompare());
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(rest);
        }






        public async Task<MultiSeriesGetRp> GetDailySeriesById(int serviceId, DateTime start, DateTime end)
        {
            var service = await this._dbContext.Services.Where(c => c.Id == serviceId).SingleAsync();
            var serviceMaps = await this._dbContext.ServiceMaps.Include(c => c.Feature).ThenInclude(c => c.Indicators).Where(c => c.Service.Id == serviceId).ToListAsync();

            foreach (var map in serviceMaps)
            {
                var entity = await this._dbContext.Features.Include(c => c.Indicators).ThenInclude(c => c.Source).SingleAsync(c => c.Id == map.Feature.Id);

                foreach (var indicator in entity.Indicators)
                {
                    var sourceItems = this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                    indicator.Source.SourceItems = sourceItems;
                }
                map.Feature = entity;
            }

            service.FeatureMap = serviceMaps;

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = service.Name,
                Avatar = service.Avatar
            };

            var aggregator = new ServiceDailyAvailabilityAggregate(service, start, end);

            var (_, availability, features) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = service.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in features)
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

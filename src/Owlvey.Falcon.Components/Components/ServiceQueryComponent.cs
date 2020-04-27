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
using Owlvey.Falcon.Repositories.Products;
using System.ComponentModel.DataAnnotations;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Components
{
    public class ServiceQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        

        public ServiceQueryComponent(FalconDbContext dbContext,
            IDateTimeGateway dateTimeGateway,
            IMapper mapper,
            IUserIdentityGateway identityService, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;            
        }

        public async Task<IEnumerable<ServiceGetListRp>> GetServicesWithAvailabilityByGroup(int productId, DateTime start, DateTime end, string group)
        {
            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(start, end);
            var entity  = await this._dbContext.FullLoadProduct(productId);

            var targetServices = entity.Services.Where(c => string.Equals(c.Group, group,StringComparison.InvariantCultureIgnoreCase)).ToList();

            var result = new List<ServiceGetListRp>();
            foreach (var item in targetServices)
            {

                var tmp = this._mapper.Map<ServiceGetListRp>(item);
                tmp.Quality = await this.MeasureAvailability(item, start, end);                
                tmp.Deploy = QualityUtils.BudgetToAction(tmp.Budget);                
                result.Add(tmp);
            }

            entity.ClearSourceItems();

            foreach (var item in targetServices)
            {
                var tmp = result.Where(c => c.Id == item.Id).SingleOrDefault();
                if (tmp != null) {
                    tmp.Previous = await this.MeasureAvailability(item, ps, pe);
                }                
            }
            return result;
        }

        public async Task<IEnumerable<ServiceGetListRp>> GetServicesWithAvailability(int productId, DateTime start, DateTime end)
        {            
            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(start, end); 

            var entity = await this._dbContext.FullLoadProduct(productId);
            var sourceItems = await this._dbContext.GetSourceItemsByProduct(productId, start, end);
            foreach (var service in entity.Services)
            {
                foreach (var map in service.FeatureMap)
                {
                    foreach (var indicator in map.Feature.Indicators)
                    {                        
                        indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                    }
                }
            }            

            var result = new List<ServiceGetListRp>();
            foreach (var item in entity.Services)
            {
                item.MeasureQuality();               

                var tmp = this._mapper.Map<ServiceGetListRp>(item);                
                tmp.Deploy = QualityUtils.BudgetToAction(tmp.Budget); 
                result.Add(tmp);
            }
            entity.ClearSourceItems();
            
            sourceItems = await this._dbContext.GetSourceItemsByProduct(productId, ps, pe);
            foreach (var service in entity.Services)
            {
                foreach (var map in service.FeatureMap)
                {
                    foreach (var indicator in map.Feature.Indicators)
                    {
                        indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                    }
                }
            }
            foreach (var item in entity.Services)
            {
                var tmp = result.Where(c => c.Id == item.Id).SingleOrDefault();
                if (tmp != null) {
                    item.MeasureQuality();                    
                    tmp.Previous = item.Quality;
                }                
            }
            return result;
        }

        public async Task<ServiceGetRp> GetServiceById(int id)
        {
            var entity = await this._dbContext.GetService(id);            
            
            if (entity == null)
                return null;
            var model = this._mapper.Map<ServiceGetRp>(entity);
            model.MergeFeaturesFrom(entity.FeatureMap.Select(c=>c.Feature));            
            return model;
        }
                
        public async Task<ServiceGetRp> GetServiceByIdWithAvailabilities(int id, DateTime start, DateTime end)
        {
            var period = new DatePeriodValue(start, end);

            var (before, previous) = period.CalculateBeforePreviousDates();

            var entity = await this._dbContext.FullGetServiceWithSourceItems(id, before.Start, end);

            if (entity == null)
                return null;                    
            
            var model = this._mapper.Map<ServiceGetRp>(entity);                        

            foreach (var map in entity.FeatureMap)
            {                
                var tmp = this._mapper.Map<SequenceFeatureGetListRp>(map.Feature);                
                var feateureAgg = new FeatureAvailabilityAggregate(map.Feature);
                var measure = feateureAgg.MeasureQuality(start, end);                
                tmp.Quality = measure.Quality;
                tmp.Availability = measure.Availability;
                tmp.Latency = measure.Latency;                                
                tmp.MapId = map.Id.Value;
                model.Features.Add(tmp);
            }            
            
            var agg = new ServiceQualityAggregate(entity);
            model.Availability = agg.MeasureQuality(start, end).Quality;
            model.PreviousAvailability = agg.MeasureQuality(previous.Start, previous.End).Quality;
            model.PreviousAvailabilityII = agg.MeasureQuality(before.Start, before.End).Quality;
            return model;
        }
        private async Task<decimal> MeasureAvailability(ServiceEntity entity, DateTime start, DateTime end)
        {
            var sources = entity.FeatureMap.SelectMany(c => c.Feature.Indicators).Select(c => c.SourceId).Distinct().ToList();
            var sourceItems = await this._dbContext.GetSourceItems(sources, start, end);

            foreach (var map in entity.FeatureMap)
            {
                foreach (var indicator in map.Feature.Indicators)
                {                    
                    indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                }
            }            
            entity.MeasureQuality();
            return entity.Quality;
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
            var service = await this._dbContext.GetService(serviceId);

            var sourceIds = service.FeatureMap.SelectMany(c => c.Feature.Indicators)
                .Select(c => c.SourceId).Distinct();

            var sourceItems = await this._dbContext.GetSourceItems(sourceIds, start, end);

            foreach (var map in service.FeatureMap)
            {
                foreach (var indicator in map.Feature.Indicators)
                {                    
                    indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                }                
            }            

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = service.Name,
                Avatar = service.Avatar,
                SLO = service.Slo
            };

            var aggregator = new ServiceDailyAggregate(service, new DatePeriodValue(start, end));

            var (availability, features) = aggregator.MeasureQuality();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Quality",
                Avatar = service.Avatar,
                Items = availability.OrderBy(c => c.Date).Select(c => new SeriesItemGetRp(c.Date, c.Measure.Quality)).ToList()
            }); ;

            foreach (var indicator in features)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("Id:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.OrderBy(c => c.Date).Select(c => new SeriesItemGetRp(c.Date, c.Measure.Quality)).ToList()
                }); 
            }
            return result;
        }

        #region Graphs


        public async Task<GraphGetRp> GetGraph(int id, DateTime start, DateTime end)
        {

            GraphGetRp result = new GraphGetRp();

            var rootService =  await this._dbContext.Services
                .Include(c=>c.FeatureMap)                                
                .SingleAsync(c => c.Id == id);

            var rootFeatures = rootService.FeatureMap.Select(c => c.FeatureId).Distinct().ToList();

            var extendedServicesIds = await this._dbContext.ServiceMaps
                .Where(c => rootFeatures.Contains(c.FeatureId))
                .Select(c => c.ServiceId)
                .ToListAsync();

            var extendedServices = await this._dbContext.Services
                .Include(c => c.FeatureMap)                
                .Where(c => extendedServicesIds.Contains(c.Id.Value))
                .ToListAsync();


            var featuresExtended = extendedServices.SelectMany(c => c.FeatureMap.Select(d => d.FeatureId));
            var featuresRoot = rootService.FeatureMap.Select(c => c.FeatureId);
            var featuresIds = featuresExtended.Union(featuresRoot).ToList();


            var features = await this._dbContext.Features
                .Include(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .Where( c=> featuresIds.Contains(c.Id.Value))
                .ToListAsync();

            var sources = features.SelectMany(c => c.Indicators.Select(d => d.SourceId));
            
            var sourceItems = await this._dbContext.GetSourceItems(sources, start, end);
            
            var services = new List<ServiceEntity>(extendedServices);
            services.Add(rootService);


            foreach (var feature in features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.Source.Id).ToList();
                }
                feature.MeasureAvailability();
            }

            foreach (var service in services)
            {
                foreach (var map in service.FeatureMap)
                {
                    var temp_feature = features.Where(c => c.Id == map.FeatureId).Single();
                    map.Feature = temp_feature;

                    if (temp_feature.ServiceMaps.Where(
                        c => c.FeatureId == temp_feature.Id.Value &&
                             c.ServiceId == service.Id.Value
                        ).Count() == 0) {
                        temp_feature.ServiceMaps.Add(new ServiceMapEntity()
                        {
                            FeatureId = temp_feature.Id.Value,
                            ServiceId = service.Id.Value,
                            Feature = temp_feature,
                            Service = service
                        });
                    }                    
                }
                service.MeasureQuality();
            }


            var snode = new GraphNode("services", "service",
                    rootService.Id.Value,
                    rootService.Avatar,
                    string.Format("{0} [ {1} | {2} ]", rootService.Name,
                    Math.Round(rootService.Slo, 2),
                    Math.Round(rootService.Quality, 2))
                    ,rootService.Quality, rootService.Slo);
            
            result.Nodes.Add(snode);

            foreach (var map in rootService.FeatureMap)
            {
                var feature = map.Feature;
                var Id = string.Format("feature_{0}", feature.Id);
                var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                if (fnode == null)
                {
                    fnode = new GraphNode("features", "feature", feature.Id.Value,
                        feature.Avatar,                        
                        feature.Name,                        
                        feature.Availability, 0);

                    result.Nodes.Add(fnode);
                }

                var fedge = new GraphEdge(snode.Id, fnode.Id,
                        fnode.Value - rootService.Slo,
                        new Dictionary<string, object>() {
                            { "Availability", fnode.Value }
                        });
                
                result.Edges.Add(fedge);

                foreach (var extended in extendedServices)
                {
                    var temporal = extended.FeatureMap.Where(c => c.FeatureId == feature.Id).SingleOrDefault();
                    if (temporal != null && temporal.Feature.ServiceMaps.Count <= 10) {
                        var temp_node = new GraphNode("services",
                            "service",
                            extended.Id.Value,
                            extended.Avatar,
                        string.Format("{0} [ {1} | {2} ]", extended.Name,
                        Math.Round(extended.Slo, 2),
                        Math.Round(extended.Quality, 2))
                        , extended.Quality, extended.Slo);

                        if (result.Nodes.Count(c => c.Id == temp_node.Id) == 0)
                        {
                            result.Nodes.Add(temp_node);

                            var tmp_edge = new GraphEdge(
                                temp_node.Id, 
                                fnode.Id, 
                                fnode.Value - extended.Slo,
                                new Dictionary<string, object>() {
                                    { "Availability", fnode.Value }
                                });
                            result.Edges.Add(tmp_edge);
                        }                        
                    }                    
                }
            }
           
            return result;
        }


        #endregion
    }
}

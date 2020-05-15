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
using System.IO.Pipes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Text.RegularExpressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Table.PivotTable;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

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

        public async Task<MultiSerieItemGetRp> GetAnnualServiceGroupCalendarReport(int serviceId, DateTime start)
        {
            var result = new MultiSerieItemGetRp();
            var period = DatePeriodValue.ToYearFromStart(start);
            var service = await this._dbContext.FullServiceWithSourceItems(serviceId, period.Start, period.End);
            var days = period.ToDaysPeriods();
            result.Avatar = service.Avatar;
            result.Name = service.Name;
            foreach (var day in days)
            {
                var measure = service.MeasureQuality(day);
                result.Items.Add(new SeriesItemGetRp(day.Start, measure.Quality));
            }
            return result;
        }

        public async Task<IEnumerable<MultiSerieItemGetRp>> GetAnnualServiceGroupCalendarReport(int productId, string group, DateTime start)
        {            
            var period = DatePeriodValue.ToYearFromStart(start);
            var product = await this._dbContext.FullLoadProductWithGroupAndSourceItems(productId, group, period.Start, period.End);
            var result = new List<MultiSerieItemGetRp>();

            var root = new MultiSerieItemGetRp();
            root.Name = group; 

            var days = period.ToDaysPeriods();                        
            foreach (var day in days)
            {   
                root.Items.Add(                
                    new SeriesItemGetRp(day.Start, product.Services.Select(c => c.MeasureQuality(day).Debt).Sum())
                );
            }

            result.Add(root);
            return result;
        }

        public async Task<AnnualServiceGroupListRp> GetAnnualServiceGroupReport(int productId, DateTime start)
        {
            var result = new AnnualServiceGroupListRp();
            var period = DatePeriodValue.ToYearFromStart(start);

            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);

            var months = period.ToYearPeriods();
            var serviceGroups = product.Services.GroupBy(c => c.Group).ToList();
            var pivot = new Dictionary<string, List<(int count, decimal quality, decimal availability, decimal latency, decimal experience)>>();
            
            foreach (var monthperiod in months)
            {
                var groupsReport = new List<(string name, int count, decimal quality, decimal availability, decimal latency, decimal experience)>();
                foreach (var group in serviceGroups)
                {                   
                    var measures = group.Select(c => new { measure = c.MeasureQuality(monthperiod), slo = c.Slo }).ToList();                                        
                    groupsReport.Add(( group.Key, group.Count(), measures.Sum(c => c.measure.Debt),
                        measures.Sum(c => c.measure.AvailabilityDebt),
                        measures.Sum(c => c.measure.LatencyDebt),
                        measures.Sum(c => c.measure.ExperienceDebt)
                        )); 
                }                
                
                foreach (var group in groupsReport)
                {
                    if (!pivot.ContainsKey(group.name)) {
                        pivot.Add(group.name, new List<(int count, decimal quality, decimal availability, decimal latency, decimal experience)>()); 
                    }
                    pivot[group.name].Add( (group.count, group.quality, group.availability, group.latency, group.experience) );
                }
                if (!pivot.ContainsKey("[Total]")) {
                    pivot.Add("[Total]", new List<(int count, decimal quality, decimal availability, decimal latency, decimal experience)>());
                }
                pivot["[Total]"].Add(
                    (
                        groupsReport.Sum(c => c.count),
                        groupsReport.Sum(c => c.quality),
                        groupsReport.Sum(c => c.availability),
                        groupsReport.Sum(c => c.latency),
                        groupsReport.Sum(c => c.experience)
                    ));
            }

            if (pivot.Count == 0) return result;

            result.Quality = pivot.Select(c =>
            {
                var tmp = new MonthRp() { Name = c.Key, Count = c.Value.ElementAt(0).count };
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].quality);
                }                
                return tmp;
            }).ToList();

            result.Availability = pivot.Select(c =>
            {
                var tmp = new MonthRp() { Name = c.Key, Count = c.Value.ElementAt(0).count };
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].availability);
                }
                return tmp;
            }).ToList();

            result.Latency = pivot.Select(c =>
            {
                var tmp = new MonthRp() { Name = c.Key, Count = c.Value.ElementAt(0).count };
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].latency);
                }
                return tmp;
            }).ToList();

            result.Experience = pivot.Select(c =>
            {
                var tmp = new MonthRp() { Name = c.Key, Count = c.Value.ElementAt(0).count };
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].experience);
                }
                return tmp;
            }).ToList();


            var weeks = period.ToWeeksPeriods();
            pivot = new Dictionary<string, List<(int count, decimal quality, decimal availability, decimal latency, decimal experience)>>();
            foreach (var week in weeks)
            {
                var groupsReport = new List<(string name, int count, decimal quality, decimal availability, decimal latency, decimal experience)>();
                foreach (var group in serviceGroups)
                {
                    var temp = new ServiceGroupListRp.ServiceGrouptem();                    
                    var measures = group.Select(c => new { measure = c.MeasureQuality(week), slo = c.Slo });
                    groupsReport.Add((group.Key, group.Count(), measures.Sum(c => c.measure.Debt),
                            measures.Sum(c => c.measure.AvailabilityDebt),
                            measures.Sum(c => c.measure.LatencyDebt),
                            measures.Sum(c => c.measure.ExperienceDebt)));
                }
                foreach (var group in groupsReport)
                {
                    if (!pivot.ContainsKey(group.name))
                    {
                        pivot.Add(group.name, new List<(int count, decimal quality, decimal availability, decimal latency, decimal experience)>());
                    }
                    pivot[group.name].Add((group.count, group.quality, group.availability, group.latency, group.experience));
                }
                
            }                       
                        
            foreach (var item in serviceGroups)
            {
                var serie = new MultiSerieItemGetRp
                {
                    Name = item.Key
                };
                for (int i = 0; i < weeks.Count; i++) {
                    serie.Items.Add(
                        new SeriesItemGetRp(weeks[i].End, 
                        pivot[item.Key][i].quality));
                }                
                result.Weekly.Add(serie);                
            }
            return result;
        }

        #region Service Group 
        public async Task<ServiceGroupListRp> GetServiceGroupReport(int productId, DatePeriodValue period)
        {
            var (_, _, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(period.Start, period.End);
            var entity = await this._dbContext.FullLoadProductWithSourceItems(productId, ps, period.End);
            var result = new ServiceGroupListRp();

            var days = period.ToDaysPeriods();

            foreach (var group in entity.Services.GroupBy(c => c.Group))
            {
                var serie = new MultiSerieItemGetRp()
                {
                    Name = group.Key
                };
                foreach (var day in days)
                {
                    serie.Items.Add(new SeriesItemGetRp(day.Start,
                        group.Select(c => c.MeasureQuality(day).Debt).Sum()));
                }

                var measures = group.Select(c => new { measure = c.MeasureQuality(period), slo = c.Slo }).ToList();
                var temp = new ServiceGroupListRp.ServiceGrouptem
                {
                    Name = group.Key,
                    SloAvg = QualityUtils.CalculateAverage(group.Select(c => c.Slo)),
                    SloMin = QualityUtils.CalculateMinimum(group.Select(c => c.Slo))
                };
                temp.QualityAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Quality));
                temp.QualityMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Quality));
                temp.AvailabilityAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Availability));
                temp.AvailabilityMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Availability));
                temp.LatencyAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Latency));
                temp.LatencyMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Latency));
                temp.ExperienceAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Experience));
                temp.ExperienceMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Experience));

                temp.Count = group.Count();
                temp.ErrorBudget = measures.Where(c => c.measure.ErrorBudget < 0).Sum(c => c.measure.ErrorBudget);
                var previous = group.Select(c => new { measure = c.MeasureQuality(new DatePeriodValue(ps, pe)), slo = c.Slo });
                temp.Previous = previous.Where(c => c.measure.ErrorBudget < 0).Sum(c => c.measure.ErrorBudget);

                result.Series.Add(serie);
                result.Items.Add(temp);
            }
            return result;
        }

        public async Task<IEnumerable<MultiSerieItemGetRp>> GetServiceGroupDailyErrorBudget(int productId, 
            DatePeriodValue period, string group) {

            var result = new List<MultiSerieItemGetRp>();
            var product = await this._dbContext.FullLoadProductWithGroupAndSourceItems(productId, group, period.Start, period.End);
            var days = period.ToDaysPeriods();
            foreach (var service in product.Services)
            {                
                result.Add(new MultiSerieItemGetRp()
                {
                     Name = service.Name,
                     Avatar = service.Avatar,                       
                     Items = days.Select(c => new SeriesItemGetRp(c.Start, service.MeasureQuality(c).Debt)).ToList()
                });                
            }
            return result;            
        }


        #endregion

        public async Task<IEnumerable<ServiceGetListRp>> GetServicesWithAvailability(int productId, DateTime start, DateTime end)
        {            
            var (_, _, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(start, end);

            var entity = await this._dbContext.FullLoadProductWithSourceItems(productId, ps, end);
                        
            var result = new List<ServiceGetListRp>();

            foreach (var service in entity.Services)
            {
                var tmp = this._mapper.Map<ServiceGetListRp>(service);
                tmp.LoadMeasure(service.MeasureQuality(new DatePeriodValue(start, end)));                                
                tmp.Deploy = QualityUtils.BudgetToAction(tmp.Budget);
                tmp.Previous = service.MeasureQuality(new DatePeriodValue(ps, pe)).Quality;
                result.Add(tmp);
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

            var entity = await this._dbContext.FullServiceWithSourceItems(id, before.Start, end);

            if (entity == null)
                return null;                    
            
            var model = this._mapper.Map<ServiceGetRp>(entity);                        

            foreach (var map in entity.FeatureMap)
            {                
                var tmp = this._mapper.Map<SequenceFeatureGetListRp>(map.Feature);                                
                var measure = map.Feature.MeasureQuality( new DatePeriodValue( start, end));                
                tmp.Quality = measure.Quality;
                tmp.Availability = measure.Availability;
                tmp.Latency = measure.Latency;
                tmp.Experience = measure.Experience;
                tmp.MapId = map.Id.Value;
                model.Features.Add(tmp);
            }            
                        
            model.Quality = entity.MeasureQuality(new DatePeriodValue( start, end)).Quality;
            model.PreviousQuality = entity.MeasureQuality(new DatePeriodValue(previous.Start, previous.End)).Quality; 
            model.PreviousQualityII = entity.MeasureQuality(new DatePeriodValue(before.Start, before.End)).Quality;
            return model;
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

        public async Task<MultiSeriesGetRp> GetDailySeriesById(int serviceId, DateTime start, 
            DateTime end)
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


        public async Task<GraphGetRp> GetGraph(int id, DatePeriodValue period)
        {

            GraphGetRp result = new GraphGetRp();

            var rootService =  await this._dbContext.Services
                .Include(c=>c.FeatureMap)                                
                .SingleAsync(c => c.Id == id);

            var rootFeatures = rootService.FeatureMap.Select(c => c.FeatureId).Distinct().ToList();

            var extendedServicesIds = await this._dbContext.ServiceMaps
                .Where(c => rootFeatures.Contains(c.FeatureId)).Select(c => c.ServiceId)
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
            
            var sourceItems = await this._dbContext.GetSourceItems(sources, period.Start, period.End);
            
            var services = new List<ServiceEntity>(extendedServices);
            services.Add(rootService);


            foreach (var feature in features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.Source.Id).ToList();
                }
                feature.MeasureQuality();
            }

            foreach (var service in services)
            {
                foreach (var map in service.FeatureMap)
                {                    
                    map.Feature = features.Where(c => c.Id == map.FeatureId).Single();

                    if (map.Feature.ServiceMaps.Where(
                        c => c.FeatureId == map.Feature.Id.Value &&
                             c.ServiceId == service.Id.Value
                        ).Count() == 0) {
                        map.Feature.ServiceMaps.Add(new ServiceMapEntity()
                        {
                            FeatureId = map.Feature.Id.Value,
                            ServiceId = service.Id.Value,
                            Feature = map.Feature,
                            Service = service
                        });
                    }                    
                }                
            }

            var rootMeasure = rootService.MeasureQuality();

            var snode = new GraphNode("services", "service",
                    rootService.Id.Value,
                    rootService.Avatar,
                    string.Format("{0} [ {1} | {2} ]", rootService.Name,
                    Math.Round(rootService.Slo, 2),
                    Math.Round(rootMeasure.Quality, 2))
                    , rootMeasure.Quality, rootService.Slo);
            
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
                        feature.MeasureQuality().Quality, 0);

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

                        var extendedMeasure = extended.MeasureQuality();

                        var temp_node = new GraphNode("services",
                            "service",
                            extended.Id.Value,
                            extended.Avatar,
                        string.Format("{0} [ {1} | {2} ]", extended.Name,
                        Math.Round(extended.Slo, 2),
                        Math.Round(extendedMeasure.Quality, 2))
                        , extendedMeasure.Quality, extended.Slo);

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

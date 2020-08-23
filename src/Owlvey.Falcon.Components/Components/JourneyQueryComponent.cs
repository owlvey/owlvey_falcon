using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Owlvey.Falcon.Repositories.Journeys;
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
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Owlvey.Falcon.Core.Models.Series;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Owlvey.Falcon.Components
{
    public class JourneyQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        

        public JourneyQueryComponent(FalconDbContext dbContext,
            IDateTimeGateway dateTimeGateway,
            IMapper mapper,
            IUserIdentityGateway identityGateway, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;            
        }

        public async Task<MultiSerieItemGetRp> GetAnnualJourneyGroupCalendarReport(int journeyId, DateTime start)
        {
            var result = new MultiSerieItemGetRp();
            var period = DatePeriodValue.ToYearFromStart(start);
            var journey = await this._dbContext.FullJourneyWithSourceItems(journeyId, period.Start, period.End);
            var days = period.ToDaysPeriods();
            result.Avatar = journey.Avatar;
            result.Name = journey.Name;
            foreach (var day in days)
            {
                var measure = journey.Measure(day);
                result.Items.Add(new SeriesItemGetRp(day.Start, measure.Availability));
            }
            return result;
        }

        public async Task<IEnumerable<MultiSerieItemGetRp>> GetAnnualJourneyGroupCalendarReport(int productId, string group, DateTime start)
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
                    new SeriesItemGetRp(day.Start, 
                    product.Journeys.Select(c => c.Measure(day).AvailabilityDebt).Sum())
                );
            }

            result.Add(root);
            return result;
        }

        public async Task<AnnualJourneyListRp> GetAnnualReport(int productId, DateTime start) {
            var result = new AnnualJourneyListRp();
            var period = DatePeriodValue.ToYearFromStart(start);
            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            var months = period.ToYearPeriods();
            
            var pivot = new Dictionary<JourneyEntity, List<JourneyQualityMeasureValue>>();
            foreach (var monthperiod in months)
            {
                foreach (var journey in product.Journeys)
                {
                    var measure = journey.Measure(monthperiod);                    
                    if (!pivot.ContainsKey(journey))
                    {
                        pivot.Add(journey, new List<JourneyQualityMeasureValue>());
                    }
                    pivot[journey].Add(measure);
                }
            }
            if (pivot.Count == 0) return result;
            result.Availability = pivot.Select(c =>
            {
                var tmp = new MonthRp() {  Id = c.Key.Id.Value, Name = string.Format("{0} | SLO:{1}", c.Key.Name, c.Key.AvailabilitySlo)};
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].Availability);
                }
                return tmp;
            }).ToList();

            result.Latency = pivot.Select(c =>
            {
                var tmp = new MonthRp() { Id = c.Key.Id.Value, Name = string.Format("{0} | SLO:{1}", c.Key.Name, c.Key.LatencySlo) };
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].Latency);
                }
                return tmp;
            }).ToList();

            result.Experience = pivot.Select(c =>
            {
                var tmp = new MonthRp() { Id = c.Key.Id.Value, Name = string.Format("{0} | SLO:{1}", c.Key.Name, c.Key.ExperienceSlo) };
                for (int i = 0; i < c.Value.Count; i++)
                {
                    tmp.SetMonthValue(i, c.Value[i].Experience);
                }
                return tmp;
            }).ToList();
            return result;
        }

        public async Task<AnnualJourneyGroupListRp> GetAnnualGroupReport(int productId, DateTime start)
        {
            var result = new AnnualJourneyGroupListRp();
            var period = DatePeriodValue.ToYearFromStart(start);

            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);

            var months = period.ToYearPeriods();
            var journeysGroups = product.Journeys.GroupBy(c => c.Group).ToList();
            var pivot = new Dictionary<string, List<(int count, decimal availability, decimal latency, decimal experience)>>();
            
            foreach (var monthperiod in months)
            {
                var groupsReport = new List<(string name, int count, decimal availability, decimal latency, decimal experience)>();
                foreach (var group in journeysGroups)
                {                   
                    var measures = group.Select(c => new { measure = c.Measure(monthperiod), slo = c.AvailabilitySlo }).ToList();                                        
                    groupsReport.Add(( group.Key, group.Count(),                         
                        measures.Sum(c => c.measure.AvailabilityDebt),
                        measures.Sum(c => c.measure.LatencyDebt),
                        measures.Sum(c => c.measure.ExperienceDebt)
                        )); 
                }                
                
                foreach (var group in groupsReport)
                {
                    if (!pivot.ContainsKey(group.name)) {
                        pivot.Add(group.name, new List<(int count, decimal availability, decimal latency, decimal experience)>()); 
                    }
                    pivot[group.name].Add( (group.count, group.availability, group.latency, group.experience) );
                }
                if (!pivot.ContainsKey("[Total]")) {
                    pivot.Add("[Total]", new List<(int count, decimal availability, decimal latency, decimal experience)>());
                }
                pivot["[Total]"].Add(
                    (
                        groupsReport.Sum(c => c.count),                        
                        groupsReport.Sum(c => c.availability),
                        groupsReport.Sum(c => c.latency),
                        groupsReport.Sum(c => c.experience)
                    ));
            }

            if (pivot.Count == 0) return result;
            
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
            var days = period.ToDaysPeriods();

            var dayli_measures =  days.Select(date => ( date,  product.Journeys.Select(d => d.Measure(date)))).ToList();
            foreach (var item in dayli_measures)
            {
                result.Series.Availability.Items.Add(new DatetimeSerieItemModel(item.date.Start, item.Item2.Sum(c=>c.AvailabilityDebt)));
                result.Series.Latency.Items.Add(new DatetimeSerieItemModel(item.date.Start, item.Item2.Sum(c => c.LatencyDebt)));
                result.Series.Experience.Items.Add(new DatetimeSerieItemModel(item.date.Start, item.Item2.Sum(c => c.ExperienceDebt)));
            }
            
            foreach (var item in journeysGroups)            
            {
                var measures_weeks = weeks.Select(week => (week, item.Select(c => c.Measure(week))));
                
                var temp = new DatetimeSerieListModel(item.Key);                
                temp.Items.AddRange(measures_weeks.Select(c => new DatetimeSerieItemModel(c.week.Start, c.Item2.Sum(d => d.AvailabilityDebt))).ToList());
                result.Series.AvailabilityDetail.Add(temp);

                var tempLatency = new DatetimeSerieListModel(item.Key);
                tempLatency.Items.AddRange(measures_weeks.Select(c => new DatetimeSerieItemModel(c.week.Start, c.Item2.Sum(d => d.LatencyDebt))).ToList());
                result.Series.LatencyDetail.Add(tempLatency);

                var tempExperience = new DatetimeSerieListModel(item.Key);
                tempExperience.Items.AddRange(measures_weeks.Select(c => new DatetimeSerieItemModel(c.week.Start, c.Item2.Sum(d => d.ExperienceDebt))).ToList());
                result.Series.ExperienceDetail.Add(tempExperience);

            }
            return result;
        }

        #region Journey Group 
        public async Task<JourneyGroupListRp> GetJourneyGroupReport(int productId, DatePeriodValue period)
        {
            
            var entity = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            var result = new JourneyGroupListRp();

            var days = period.ToDaysPeriods();

            foreach (var group in entity.Journeys.GroupBy(c => c.Group))
            {
                var serie = new MultiSerieItemGetRp()
                {
                    Name = group.Key
                };
                foreach (var day in days)
                {
                    serie.Items.Add(new SeriesItemGetRp(day.Start,
                        group.Select(c => c.Measure(day).AvailabilityDebt).Sum()));
                }

                var measures = group.Select(c => 
                        new { measure = c.Measure(period)}).ToList();
                var temp = new JourneyGroupListRp.JourneyGrouptem
                {
                    Name = group.Key,
                    AvailabilitySloAvg = QualityUtils.CalculateAverage(group.Select(c => c.AvailabilitySlo)),
                    AvailabilitySloMin = QualityUtils.CalculateMinimum(group.Select(c => c.AvailabilitySlo)),

                    LatencySloAvg = QualityUtils.CalculateAverage(group.Select(c => c.LatencySlo)),
                    LatencySloMin = QualityUtils.CalculateMinimum(group.Select(c => c.LatencySlo)),

                    ExperienceSloAvg = QualityUtils.CalculateAverage(group.Select(c => c.ExperienceSlo)),
                    ExperienceSloMin = QualityUtils.CalculateMinimum(group.Select(c => c.ExperienceSlo))
                };
                
                temp.AvailabilityAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Availability));
                temp.AvailabilityMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Availability));
                temp.LatencyAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Latency));
                temp.LatencyMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Latency));
                temp.ExperienceAvg = QualityUtils.CalculateAverage(measures.Select(c => c.measure.Experience));
                temp.ExperienceMin = QualityUtils.CalculateMinimum(measures.Select(c => c.measure.Experience));

                temp.Count = group.Count();
                temp.AvailabilityDebt = measures.Sum(c => c.measure.AvailabilityDebt);
                temp.ExperienceDebt = measures.Sum(c => c.measure.ExperienceDebt);
                temp.LatencyDebt = measures.Sum(c => c.measure.LatencyDebt);

                result.Series.Add(serie);
                result.Items.Add(temp);
            }
            return result;
        }

        public async Task<DatetimeSerieModel> GetJourneyGroupDailyErrorBudget(int productId, 
            DatePeriodValue period, string group) {

            var product = await this._dbContext.FullLoadProductWithGroupAndSourceItems(productId, group, period.Start, period.End);

            var result = new DatetimeSerieModel();
            result.Start = period.Start;
            result.End = period.End;
            result.Name = product.Name;
            var days = period.ToDaysPeriods();

            foreach (var item in days)
            {
                var temp = product.Journeys.Select(c => c.Measure(item));
                result.Availability.Items.Add(new DatetimeSerieItemModel(item.Start, temp.Sum(c => c.AvailabilityDebt)));
                result.Latency.Items.Add(new DatetimeSerieItemModel(item.Start, temp.Sum(c => c.LatencyDebt)));
                result.Experience.Items.Add(new DatetimeSerieItemModel(item.Start, temp.Sum(c => c.ExperienceDebt)));
            }

            foreach (var journey in product.Journeys)
            {
                var temp = new DatetimeSerieListModel()
                {
                    Name = journey.Name,
                    Avatar = journey.Avatar
                };
                temp.AddItems(days.Select(c => (c.Start, journey.Measure(c).AvailabilityDebt)).ToList());                
                result.AvailabilityDetail.Add(temp);

                var tempL = new DatetimeSerieListModel()
                {
                    Name = journey.Name,
                    Avatar = journey.Avatar
                };
                tempL.AddItems(days.Select(c => (c.Start, journey.Measure(c).LatencyDebt)).ToList());
                result.LatencyDetail.Add(tempL);

                var tempE = new DatetimeSerieListModel()
                {
                    Name = journey.Name,
                    Avatar = journey.Avatar
                };
                tempE.AddItems(days.Select(c => (c.Start, journey.Measure(c).ExperienceDebt)).ToList());
                result.ExperienceDetail.Add(tempE);
            }
            return result;            
        }


        #endregion

        public async Task<IEnumerable<JourneyGetListRp>> GetJourneysWithAvailability(int productId, DateTime start, DateTime end)
        {   
            var entity = await this._dbContext.FullLoadProductWithSourceItems(productId, start, end);
                        
            var result = new List<JourneyGetListRp>();

            foreach (var journey in entity.Journeys)
            {
                var tmp = this._mapper.Map<JourneyGetListRp>(journey);                
                tmp.LoadMeasure(journey.Measure(new DatePeriodValue(start, end)));                       
                result.Add(tmp);
            }

            return result;
        }

        public async Task<JourneyGetRp> GetJourneyById(int id)
        {
            var entity = await this._dbContext.GetJourney(id);            
            
            if (entity == null)
                return null;
            var model = this._mapper.Map<JourneyGetRp>(entity);
            model.MergeFeaturesFrom(entity.FeatureMap.Select(c=>c.Feature));            
            return model;
        }
                
        public async Task<JourneyGetRp> GetJourneyByIdWithAvailabilities(int id, DateTime start, DateTime end)
        {
            var period = new DatePeriodValue(start, end);

            var (before, previous) = period.CalculateBeforePreviousDates();

            var entity = await this._dbContext.FullJourneyWithSourceItems(id, before.Start, end);

            if (entity == null)
                return null;                    
            
            var model = this._mapper.Map<JourneyGetRp>(entity);                        

            foreach (var map in entity.FeatureMap)
            {                
                var tmp = this._mapper.Map<SequenceFeatureGetListRp>(map.Feature);                                
                var measure = map.Feature.Measure( new DatePeriodValue( start, end));
                tmp.LoadMeasure(measure);                                
                tmp.MapId = map.Id.Value;
                model.Features.Add(tmp);
            }            
                        
            model.LoadMeasure(entity.Measure(new DatePeriodValue( start, end)));
            model.LoadPrevious(entity.Measure(new DatePeriodValue(previous.Start, previous.End)));
            model.LoadBefore(entity.Measure(new DatePeriodValue(before.Start, before.End)));
            return model;
        }
        public async Task<JourneyGetRp> GetByProductIdName(int productId, string name)
        {
            var entity = await this._dbContext.Journeys.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);
            return this._mapper.Map<JourneyGetRp>(entity);
        }

        public async Task<IEnumerable<JourneyGetListRp>> GetListByProductId(int productId)
        {
            var entities = await this._dbContext.Journeys.Include(c => c.FeatureMap).ThenInclude(c => c.Feature).Where(c => c.Product.Id.Equals(productId)).ToListAsync();

            return this._mapper.Map<IEnumerable<JourneyGetListRp>>(entities);
        }

        public async Task<IEnumerable<FeatureGetListRp>> GetFeaturesComplement(int journeyId)
        {
            var journey = await this._dbContext.Journeys.Include(c => c.FeatureMap).ThenInclude(c => c.Feature).Where(c => c.Id == journeyId).SingleAsync();

            var journeyFeatures = journey.FeatureMap.Select(c => c.Feature).ToList();
            var features = await this._dbContext.Features.Where(c => c.ProductId == journey.ProductId).ToListAsync();
            var rest = features.Except(journeyFeatures, new FeatureEntityCompare());
            return this._mapper.Map<IEnumerable<FeatureGetListRp>>(rest);
        }

        public async Task<DatetimeSerieModel> GetDailySeriesById(int journeyId, DatePeriodValue period)
        {
            var journey = await this._dbContext.GetJourney(journeyId);

            var sourceIds = journey.FeatureMap.SelectMany(c => c.Feature.Indicators)
                .Select(c => c.SourceId).Distinct();

            var sourceItems = await this._dbContext.GetSourceItems(sourceIds, period.Start, period.End);

            foreach (var map in journey.FeatureMap)
            {
                foreach (var indicator in map.Feature.Indicators)
                {                    
                    indicator.Source.SourceItems = sourceItems.Where(c=>c.SourceId == indicator.SourceId).ToList();
                }                
            }            

            var result = new DatetimeSerieModel
            {
                Start = period.Start,
                End = period.End,
                Name = journey.Name,
                Avatar = journey.Avatar,                
            };

            var aggregator = new JourneyDailyAggregate(journey, period);

            var (availability, features) = aggregator.MeasureQuality;

            result.Availability.AddItems(            
                availability.OrderBy(c => c.Date).Select(c => (c.Date, c.Measure.Availability)).ToList()
            );

            result.Latency.AddItems(
                availability.OrderBy(c => c.Date).Select(c => (c.Date, c.Measure.Latency)).ToList()
            );

            result.Experience.AddItems(
                availability.OrderBy(c => c.Date).Select(c => (c.Date, c.Measure.Experience)).ToList()
            );

            foreach (var (feature, avaValues) in features)
            {   
                var pivotAvailability = new DatetimeSerieListModel(feature.Name, feature.Avatar);
                pivotAvailability.AddItems( avaValues.OrderBy(c=>c.Date)
                    .Select(c=> (c.Date,  QualityUtils.MeasureDebt( c.Measure.Availability, journey.AvailabilitySlo))).ToList());
                result.AvailabilityDetail.Add(pivotAvailability);

                var pivotLatency = new DatetimeSerieListModel(feature.Name, feature.Avatar);
                pivotLatency.AddItems(avaValues.OrderBy(c => c.Date)
                    .Select(c => (c.Date, QualityUtils.MeasureLatencyDebt(c.Measure.Latency, journey.LatencySlo))).ToList());
                result.LatencyDetail.Add(pivotLatency);

                var pivotExperience = new DatetimeSerieListModel(feature.Name, feature.Avatar);
                pivotExperience.AddItems(avaValues.OrderBy(c => c.Date)
                    .Select(c => (c.Date, QualityUtils.MeasureDebt(c.Measure.Experience, journey.ExperienceSlo))).ToList());
                result.ExperienceDetail.Add(pivotExperience);
            }            
            return result;
        }

        #region Graphs


        public async Task<GraphGetRp> GetGraph(int id, DatePeriodValue period)
        {

            GraphGetRp result = new GraphGetRp();

            var rootJourney =  await this._dbContext.Journeys
                .Include(c=>c.FeatureMap)                                
                .SingleAsync(c => c.Id == id);

            var rootFeatures = rootJourney.FeatureMap.Select(c => c.FeatureId).Distinct().ToList();

            var extendedJourneysIds = await this._dbContext.JourneyMaps
                .Where(c => rootFeatures.Contains(c.FeatureId)).Select(c => c.JourneyId)
                .ToListAsync();

            var extendedJourneys = await this._dbContext.Journeys
                .Include(c => c.FeatureMap)                
                .Where(c => extendedJourneysIds.Contains(c.Id.Value))
                .ToListAsync();

            var featuresExtended = extendedJourneys.SelectMany(c => c.FeatureMap.Select(d => d.FeatureId));
            var featuresRoot = rootJourney.FeatureMap.Select(c => c.FeatureId);
            var featuresIds = featuresExtended.Union(featuresRoot).ToList();

            var features = await this._dbContext.Features
                .Include(c => c.Indicators)
                .ThenInclude(c => c.Source)
                .Where( c=> featuresIds.Contains(c.Id.Value))
                .ToListAsync();

            var sources = features.SelectMany(c => c.Indicators.Select(d => d.SourceId));
            
            var sourceItems = await this._dbContext.GetSourceItems(sources, period.Start, period.End);
            
            var journeys = new List<JourneyEntity>(extendedJourneys);
            journeys.Add(rootJourney);


            foreach (var feature in features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.Source.Id).ToList();
                }
                feature.Measure();
            }

            foreach (var journey in journeys)
            {
                foreach (var map in journey.FeatureMap)
                {                    
                    map.Feature = features.Where(c => c.Id == map.FeatureId).Single();

                    if (map.Feature.JourneyMaps.Where(
                        c => c.FeatureId == map.Feature.Id.Value &&
                             c.JourneyId == journey.Id.Value
                        ).Count() == 0) {
                        map.Feature.JourneyMaps.Add(new JourneyMapEntity()
                        {
                            FeatureId = map.Feature.Id.Value,
                            JourneyId = journey.Id.Value,
                            Feature = map.Feature,
                            Journey = journey
                        });
                    }                    
                }                
            }

            var rootMeasure = rootJourney.Measure();

            var snode = new GraphNode("journeyss", "journey",
                    rootJourney.Id.Value,
                    rootJourney.Avatar,
                    string.Format("{0} [ {1} | {2} ]", rootJourney.Name,
                    Math.Round(rootJourney.AvailabilitySlo, 2),
                    Math.Round(rootMeasure.Availability, 2))
                    , rootMeasure.Availability, rootJourney.AvailabilitySlo);
            
            result.Nodes.Add(snode);

            foreach (var map in rootJourney.FeatureMap)
            {
                var feature = map.Feature;
                var Id = string.Format("feature_{0}", feature.Id);
                var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                if (fnode == null)
                {
                    fnode = new GraphNode("features", "feature", feature.Id.Value,
                        feature.Avatar,                        
                        feature.Name,                        
                        feature.Measure().Availability, 0);

                    result.Nodes.Add(fnode);
                }

                var fedge = new GraphEdge(snode.Id, fnode.Id,
                        fnode.Value - rootJourney.AvailabilitySlo,
                        new Dictionary<string, object>() {
                            { "Availability", fnode.Value }
                        });
                
                result.Edges.Add(fedge);

                foreach (var extended in extendedJourneys)
                {
                    var temporal = extended.FeatureMap.Where(c => c.FeatureId == feature.Id).SingleOrDefault();
                    if (temporal != null && temporal.Feature.JourneyMaps.Count <= 10) {

                        var extendedMeasure = extended.Measure();

                        var temp_node = new GraphNode("journeys",
                            "journey",
                            extended.Id.Value,
                            extended.Avatar,
                        string.Format("{0} [ {1} | {2} ]", extended.Name,
                        Math.Round(extended.AvailabilitySlo, 2),
                        Math.Round(extendedMeasure.Availability, 2))
                        , extendedMeasure.Availability, extended.AvailabilitySlo);

                        if (result.Nodes.Count(c => c.Id == temp_node.Id) == 0)
                        {
                            result.Nodes.Add(temp_node);

                            var tmp_edge = new GraphEdge(
                                temp_node.Id, 
                                fnode.Id, 
                                fnode.Value - extended.AvailabilitySlo,
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

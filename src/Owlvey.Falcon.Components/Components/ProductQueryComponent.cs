using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Owlvey.Falcon.Repositories.Products;
using Owlvey.Falcon.Repositories.Journeys;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Owlvey.Falcon.Core.Entities.Source;
using System.Collections.Concurrent;

namespace Owlvey.Falcon.Components
{
    public class ProductQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        
        public ProductQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway,            
            IMapper mapper, IUserIdentityGateway identityGateway, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {            
            this._dbContext = dbContext;
        }

        public async Task<AnchorRp> GetAnchor(int productId, string name)
        {
            var entity = await this._dbContext.Anchors
                .Where(c => c.ProductId == productId && c.Name == name).SingleAsync();
            return this._mapper.Map<AnchorRp>(entity);
        }
        public async Task<IEnumerable<AnchorRp>> GetAnchors(int productId)
        {
            var entities = await this._dbContext.Anchors
                .Where(c => c.ProductId == productId).ToListAsync();
            return this._mapper.Map<IEnumerable<AnchorRp>>(entities);
        }

        public async Task<ProductGetRp> GetProductByName(int customerId, string Name)
        {
            var entity = await this._dbContext.Products.SingleAsync(c => c.Name == Name && c.CustomerId == customerId);
            return this._mapper.Map<ProductGetRp>(entity);
        }

        public async Task<ProductGetRp> GetProductById(int id)
        {
            var entity = await this._dbContext.Products.SingleOrDefaultAsync(c => c.Id.Equals(id));
            return this._mapper.Map<ProductGetRp>(entity);
        }


        public async Task<GraphGetRp> GetGraphAvailability(int productId, DatePeriodValue period)
        {

            GraphGetRp result = new GraphGetRp();
            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            result.Name = product.Name;
            result.Id = product.Id.Value;
            result.Avatar = product.Avatar;

            /*
            var node = new GraphNode
            {
                Id = string.Format("product_{0}", id),
                Avatar = product.Avatar,
                Name = product.Name,
                Availability = 1,
                Group = "products"
            };
            result.Nodes.Add(node);
            */

            foreach (var journey in product.Journeys)
            {
                var measure = journey.Measure();                
                var snode = new GraphNode
                {
                    Id = string.Format("journey_{0}", journey.Id),
                    Avatar = journey.Avatar,
                    Name = string.Format("{0} [ {1} | {2} ]", journey.Name,
                        Math.Round(journey.AvailabilitySlo, 2),
                        Math.Round(measure.Availability, 2)),                    
                    Value = measure.Availability,
                    Group = "journeys",
                    Slo = journey.AvailabilitySlo,                    
                    Budget = measure.AvailabilityErrorBudget
                };
                
                result.Nodes.Add(snode);                

                foreach (var map in journey.FeatureMap)
                {
                    var featureMeasure = map.Feature.Measure();
                    var Id = string.Format("feature_{0}", map.Feature.Id);
                    var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                    if (fnode == null)
                    {
                        fnode = new GraphNode
                        {
                            Id = Id,
                            Avatar = map.Feature.Avatar,
                            Name = map.Feature.Name,
                            Value = featureMeasure.Availability,
                            Group = "features"
                        };
                        result.Nodes.Add(fnode);
                    }
                    var fedge = new GraphEdge()
                    {
                        From = snode.Id,
                        To = fnode.Id,
                        Value = QualityUtils.MeasureBudget(fnode.Value, journey.AvailabilitySlo),
                        Tags = new Dictionary<string, object>() {
                            { "Availability", fnode.Value }
                        }
                    };
                    result.Edges.Add(fedge);
                }
            }
            return result;
        }

        public async Task<GraphGetRp> GetGraphLatency(int productId, DatePeriodValue period)
        {

            GraphGetRp result = new GraphGetRp();
            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            result.Name = product.Name;
            result.Id = product.Id.Value;
            result.Avatar = product.Avatar;
            foreach (var journey in product.Journeys)
            {
                var measure = journey.Measure();
                var snode = new GraphNode
                {
                    Id = string.Format("journey_{0}", journey.Id),
                    Avatar = journey.Avatar,
                    Name = string.Format("{0} [ {1} | {2} ]", journey.Name,
                        Math.Round(journey.LatencySlo, 2),
                        Math.Round(measure.Latency, 2)),
                    Value = measure.Latency,
                    Group = "journeys",
                    Slo = journey.LatencySlo,                    
                    Budget = measure.LatencyErrorBudget
                };

                result.Nodes.Add(snode);

                foreach (var map in journey.FeatureMap)
                {
                    var featureMeasure = map.Feature.Measure();
                    var Id = string.Format("feature_{0}", map.Feature.Id);
                    var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                    if (fnode == null)
                    {
                        fnode = new GraphNode
                        {
                            Id = Id,
                            Avatar = map.Feature.Avatar,
                            Name = map.Feature.Name,
                            Value = featureMeasure.Latency,
                            Group = "features"
                        };
                        result.Nodes.Add(fnode);
                    }
                    var fedge = new GraphEdge()
                    {
                        From = snode.Id,
                        To = fnode.Id,
                        Value = QualityUtils.MeasureBudget(fnode.Value, journey.LatencySlo),
                        Tags = new Dictionary<string, object>() {
                            { "Latency", fnode.Value }
                        }
                    };
                    result.Edges.Add(fedge);
                }
            }
            return result;
        }

        public async Task<GraphGetRp> GetGraphExperience(int productId, DatePeriodValue period)
        {

            GraphGetRp result = new GraphGetRp();
            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            result.Name = product.Name;
            result.Id = product.Id.Value;
            result.Avatar = product.Avatar;
            foreach (var journey in product.Journeys)
            {
                var measure = journey.Measure();
                var snode = new GraphNode
                {
                    Id = string.Format("journey_{0}", journey.Id),
                    Avatar = journey.Avatar,
                    Name = string.Format("{0} [ {1} | {2} ]", journey.Name,
                        Math.Round(journey.ExperienceSlo, 2),
                        Math.Round(measure.Experience, 2)),
                    Value = measure.Experience,
                    Group = "journeys",
                    Slo = journey.ExperienceSlo,                    
                    Budget = measure.ExperienceErrorBudget
                };

                result.Nodes.Add(snode);

                foreach (var map in journey.FeatureMap)
                {
                    var featureMeasure = map.Feature.Measure();
                    var Id = string.Format("feature_{0}", map.Feature.Id);
                    var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                    if (fnode == null)
                    {
                        fnode = new GraphNode
                        {
                            Id = Id,
                            Avatar = map.Feature.Avatar,
                            Name = map.Feature.Name,
                            Value = featureMeasure.Experience,
                            Group = "features"
                        };
                        result.Nodes.Add(fnode);
                    }
                    var fedge = new GraphEdge()
                    {
                        From = snode.Id,
                        To = fnode.Id,
                        Value = QualityUtils.MeasureBudget(fnode.Value, journey.ExperienceSlo),
                        Tags = new Dictionary<string, object>() {
                            { "Latency", fnode.Value }
                        }
                    };
                    result.Edges.Add(fedge);
                }
            }
            return result;
        }
        /// <summary>
        /// Get All Product
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductBaseRp>> GetProducts(int customerId)
        {
            var entities = await this._dbContext.Products.Where(c => c.Customer.Id == customerId).ToListAsync();
            return this._mapper.Map<IEnumerable<ProductBaseRp>>(entities);
        }
        public async Task<ProductGetListRp> GetProductsWithInformation(int customerId, DatePeriodValue period)
        {
            var products = await this._dbContext.Products.Where(c => c.CustomerId == customerId).ToListAsync();

            var (before, previous) = period.CalculateBeforePreviousDates();

            var targets = new List<ProductEntity>();
            foreach (var item in products)
            {
                targets.Add(await this._dbContext.FullLoadProductWithSourceItems(item.Id.Value, before.Start, period.End));
            }
            
            var squads = await this._dbContext.Squads
                .Include(c=>c.FeatureMaps)
                .Where(c => c.CustomerId == customerId).ToListAsync();

            var features = targets.SelectMany(c => c.Features).Distinct(new FeatureEntityCompare());

            foreach (var squad in squads.SelectMany(c=>c.FeatureMaps))
            {
                squad.Feature = features.Single(c => c.Id == squad.FeatureId);                
            }

            foreach (var feature in targets.SelectMany(c=>c.Features))
            {   
                foreach (var target in feature.Squads)
                {
                    target.Squad = squads.Single(c => c.Id == target.SquadId);
                }
            }

            var models = new ProductGetListRp();

            foreach (var item in products)
            {
                var tmp  = this._mapper.Map<ProductGetListItemRp>(item);
                var agg = new FeatureOwnershipAggregate(squads, item.Features);
                tmp.Ownership = agg.Measure().assigned;
                tmp.Debt = item.Measure(period);
                tmp.PreviousDebt = item.Measure(previous);
                tmp.BeforeDebt = item.Measure(before);
                models.Items.Add(tmp);
            }

            return models;
        }

        #region Daily Reports


        private MultiSeriesGetRp InternalGetDailyJourneySeries(ProductEntity product, DateTime start, DateTime end) {            

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = product.Name,
                Avatar = product.Avatar
            };

            var period = new DatePeriodValue(start, end);

            var agg = new ProductDailyAggregate(product, period);

            var response = agg.MeasureQuality();

            var debtSerie = new MultiSerieItemGetRp()
            {
                Name = "Debt",
                Avatar = product.Avatar
            };            
            foreach (var item in response.OrderBy(c => c.Date))
            {                
                debtSerie.Items.Add(new SeriesItemGetRp(item.Date, item.Measure.AvailabilityDebt));                                
            }
            result.Series.Add(debtSerie);            

            return result;
        }
        public async Task<MultiSeriesGetRp> GetDailyJourneysSeriesByIdAndGroup(int productId, DatePeriodValue period, string group)
        {
            var product = await this._dbContext.FullLoadProductWithGroupAndSourceItems(productId, group, period.Start, period.End);
            return this.InternalGetDailyJourneySeries(product, period.Start, period.End);
        }
        public async Task<MultiSeriesGetRp> GetDailyJourneysSeriesById(int productId, DatePeriodValue period)
        {
            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            return this.InternalGetDailyJourneySeries(product, period.Start, period.End);
        }

        #endregion

        #region Reports
        public async Task<(ProductEntity, MemoryStream)> GetProductExportToExcel(int productId,
         DateTime start, DateTime end)
        {
            var product = await this._dbContext.FullLoadProduct(productId);

            var sourceItems = await this._dbContext.GetSourceItemsByProduct(productId, start, end);

            var squadsData = await this._dbContext.SquadFeatures
                .Include(c => c.Squad)
                .Where(c => c.Feature.ProductId == productId).ToListAsync();

            var sourceExports = new List<ExportExcelSourceRp>();

            foreach (var source in product.Sources)
            {
                source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();                
                sourceExports.Add(new ExportExcelSourceRp(source));
            }            

            var featuresExports = new List<ExportExcelFeatureRp>();
            var featuresDetailExports = new List<ExportExcelFeatureDetailRp>();

            foreach (var feature in product.Features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source = product.Sources.Single(c => c.Id == indicator.SourceId);                    
                }
                foreach (var squadMap in squadsData.Where(c => c.FeatureId == feature.Id).ToList())
                {
                    squadMap.Feature = feature;
                    feature.Squads.Add(squadMap);
                }
                feature.Measure();
                featuresExports.Add(new ExportExcelFeatureRp(feature));

                foreach (var indicator in feature.Indicators)
                {                    
                    featuresDetailExports.Add(new ExportExcelFeatureDetailRp( indicator));
                }
            }
            
            var journeysExports = new List<ExportExcelJourneyRp>();
            var journeyDetailExports = new List<ExportExcelJourneyDetailRp>();
            foreach (var journey in product.Journeys)
            {
                foreach (var map in journey.FeatureMap)
                {
                    map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                }
                journey.Measure();
                journeysExports.Add(new ExportExcelJourneyRp(journey));
             
                foreach (var featureMap in journey.FeatureMap)
                {                    
                    journeyDetailExports.Add(new ExportExcelJourneyDetailRp(featureMap));
                }               
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var journeysSheet = package.Workbook.Worksheets.Add("Journeys");
                journeysSheet.Cells.LoadFromCollection(journeysExports, true);
                var journeysDetailSheet = package.Workbook.Worksheets.Add("JourneysDetail");
                journeysDetailSheet.Cells.LoadFromCollection(journeyDetailExports, true);
                var featuresSheet = package.Workbook.Worksheets.Add("Features");
                featuresSheet.Cells.LoadFromCollection(featuresExports, true);
                var indicatorsSheet = package.Workbook.Worksheets.Add("Indicators");
                indicatorsSheet.Cells.LoadFromCollection(featuresDetailExports, true);
                var sourceSheet = package.Workbook.Worksheets.Add("Sources");
                sourceSheet.Cells.LoadFromCollection(sourceExports, true);
                package.Save();
            }

            stream.Position = 0;
            return (product, stream);
        }
        #endregion

        #region Dashboard

        public async Task<ProductDashboardRp> GetJourneyGroupDashboard(int productId,
            DateTime start, DateTime end)
        {
            var result = new ProductDashboardRp();

            var product = await this._dbContext.FullLoadProduct(productId);

            
            (_, result.FeatureTotal, result.FeatureAssigned) = product.MeasureCoverage();

            (_, result.SourceTotal, result.SourceAssigned) = product.MeasureUtilization();

            var sourceItems = await this._dbContext.GetSourceItemsByProduct(productId, start, end);

            foreach (var item in product.Sources)
            {
                item.SourceItems = sourceItems.Where(c => c.SourceId == item.Id).ToList();
            }
            
            int sloFails = 0;
            var temp = new List<ProductDashboardRp.JourneyGroupRp>();            
            
            foreach (var group in product.Journeys.GroupBy(c=>c.Group))
            {
                var targetGroup = new ProductDashboardRp.JourneyGroupRp
                {
                    Total = group.Count(),
                    Name = group.Key
                };
                foreach (var journey in group)
                {                    
                    foreach (var map in journey.FeatureMap)
                    {
                        map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                    }
                    var measure = journey.Measure();
                    
                    if (measure.Availability < journey.AvailabilitySlo)
                    {
                        targetGroup.Fail += 1;
                        sloFails += 1;
                    }
                }
                temp.Add(targetGroup);                
            }

            result.Groups.Add(new ProductDashboardRp.JourneyGroupRp()
            {
                Name = "All",
                Total = product.Journeys.Count,
                Fail = sloFails
            });
            result.Groups.AddRange(temp);
            return result;
        }

        public async Task<OperationProductDashboardRp> GetProductDashboard(int productId,
            DateTime start, DateTime end)
        {
            var product = await this._dbContext.FullLoadProduct(productId);

            var squadsData = await this._dbContext.SquadFeatures
                .Include(c => c.Squad)
                .Where(c => c.Feature.ProductId == productId).ToListAsync();

            var incidentsData = await this._dbContext.IncidentMaps
                .Include(c => c.Incident)
                .Where(c => c.Feature.ProductId == productId).ToListAsync();


            var sourceItems = await this._dbContext.GetSourceItemsByProduct(productId, start, end);
            var result = new OperationProductDashboardRp();

            foreach (var source in product.Sources)
            {
                source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();                
                var measure = source.Measure();                
                
                result.Sources.Add(new SourceGetListRp()
                {
                    Id = source.Id.Value,
                    Measure = measure,                    
                    Avatar = source.Avatar,
                    CreatedBy = source.CreatedBy,
                    CreatedOn = source.CreatedOn.Value,
                    AvailabilityDefinition = source.AvailabilityDefinition,
                    LatencyDefinition = source.LatencyDefinition,
                    ExperienceDefinition = source.ExperienceDefinition,
                    Name = source.Name
                });                
            }

            foreach (var feature in product.Features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    indicator.Source = product.Sources.Single(c => c.Id == indicator.SourceId);
                }
                foreach (var squadMap in squadsData.Where(c => c.FeatureId == feature.Id).ToList())
                {
                    squadMap.Feature = feature;
                    feature.Squads.Add(squadMap);
                }
                                
                var tmp = this._mapper.Map<FeatureGetListRp>(feature);
                var measure = feature.Measure();
                tmp.LoadMeasure(measure);
                result.Features.Add(tmp);

                var featureIncidents = incidentsData.Where(c => c.FeatureId == feature.Id)
                    .Select(c => c.Incident).ToList();
                var incidentAggregate = new IncidentMetricAggregate(featureIncidents);

                var aggIncident = incidentAggregate.Metrics();
                result.IncidentInformation[feature.Id.Value] = new
                {
                    count = featureIncidents.Count,
                    mttd = aggIncident.mttd,
                    mtte = aggIncident.mtte,
                    mttf = aggIncident.mttf,
                    mttm = aggIncident.mttm
                };

                result.FeatureMaps[feature.Id.Value] = feature.Indicators.OrderBy(c=>c.Id).Select(c => c.SourceId).ToList();
                result.SquadMaps[feature.Id.Value] = squadsData.Where(c => c.FeatureId == feature.Id)
                                                     .Select(c => c.SquadId)
                                                     .Distinct().ToList();
            }

            foreach (var squad in squadsData.Select(c => c.Squad).Distinct(new SquadCompare()))
            {
                var tmp = this._mapper.Map<SquadGetListRp>(squad);
                result.Squads.Add(tmp);
            }

            int sloFails = 0;

            foreach (var journey in product.Journeys)
            {
                foreach (var map in journey.FeatureMap)
                {
                    map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                }                
                
                var measure = journey.Measure();

                if (measure.Availability < journey.AvailabilitySlo)
                {
                    sloFails += 1;
                }

                var tmp = this._mapper.Map<JourneyGetListRp>(journey);
                result.Journeys.Add(tmp);
                result.JourneyMaps[journey.Id.Value] = journey.FeatureMap.OrderBy(c=>c.Id).Select(c => c.FeatureId).ToList();
                
            }

            result.Journeys = result.Journeys.OrderBy(c => c.Availability).ToList();
            result.SLOFail = sloFails;
            result.SLOProportion = QualityUtils.CalculateFailProportion(product.Journeys.Count, sloFails);
            //TODO: change
            //result.SourceStats = new StatsValue(result.Sources.Select(c => c.Measure));            
            result.FeaturesStats = new StatsValue(result.Features.Select(c => c.Availability));
            result.FeaturesCoverage = QualityUtils.CalculateProportion(product.Features.Count,
                squadsData.Select(c=>c.FeatureId).Distinct().Count());

            result.JourneysStats = new StatsValue(result.Journeys.Select(c => c.Availability));
            return result;
        }
        #endregion

        #region Exports

        public async Task<MemoryStream> ExportAnnualAvailabilityInteractions(int productId)
        {
            var period = DatePeriodValue.ToYearFromStart(this._datetimeGateway.GetCurrentDateTime());
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
                var agg = new AnnualAvailabilityInteractionsAggregate(product, this._datetimeGateway.GetCurrentDateTime());

                var model = agg.Execute();

                var journeySheet = package.Workbook.Worksheets.Add("journeys");
                journeySheet.Cells.LoadFromCollection(model.journeys, true);

                var sourceSheet = package.Workbook.Worksheets.Add("Sources");
                sourceSheet.Cells.LoadFromCollection(model.sources, true);

                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
        public async Task<MemoryStream> ExportItems(int productId, DatePeriodValue period)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sources = await this._dbContext.Sources.Where(c=>c.ProductId == productId).ToListAsync();

                var ids = sources.Select(c => c.Id.Value).Distinct().ToList();
                var sourceItems = await this._dbContext.GetSourceItems(ids, period.Start, period.End);

                var aggregate = new Builders.SourceItemsExportBuilder(sources, sourceItems);
                var model = aggregate.Execute();

                var sourceSheet = package.Workbook.Worksheets.Add("Sources");
                sourceSheet.Cells.LoadFromCollection(model.sources, true);

                var sourceItemsSheet = package.Workbook.Worksheets.Add("SourceItems");
                sourceItemsSheet.Cells.LoadFromCollection(model.items, true);

                package.Save();
            }
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}

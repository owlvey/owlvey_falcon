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
using Owlvey.Falcon.Repositories.Services;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Owlvey.Falcon.Core.Entities.Source;

namespace Owlvey.Falcon.Components
{
    public class ProductQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly FeatureQueryComponent _featureQueryComponent;
        private readonly ServiceQueryComponent _serviceQueryComponent;        
        public ProductQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway,
            FeatureQueryComponent featureQueryComponent, ServiceQueryComponent serviceQueryComponent,
            IMapper mapper, IUserIdentityGateway identityService, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._featureQueryComponent = featureQueryComponent;
            this._serviceQueryComponent = serviceQueryComponent;
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


        public async Task<GraphGetRp> GetGraph(int productId, DatePeriodValue period)
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
            foreach (var service in product.Services)
            {
                var measure = service.MeasureQuality();
                var snode = new GraphNode
                {
                    Id = string.Format("service_{0}", service.Id),
                    Avatar = service.Avatar,
                    Name = string.Format("{0} [ {1} | {2} ]", service.Name,
                        Math.Round(service.Slo, 2),
                        Math.Round(measure.Quality, 2)),
                    Value = measure.Quality,
                    Group = "services",
                    Slo = service.Slo,
                    Importance = QualityUtils.MeasureImpact(service.Slo),
                    Budget = QualityUtils.MeasureBudget(measure.Quality, service.Slo)
                };
                
                result.Nodes.Add(snode);                

                foreach (var map in service.FeatureMap)
                {
                    var featureMeasure = map.Feature.MeasureQuality();
                    var Id = string.Format("feature_{0}", map.Feature.Id);
                    var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                    if (fnode == null)
                    {
                        fnode = new GraphNode
                        {
                            Id = Id,
                            Avatar = map.Feature.Avatar,
                            Name = map.Feature.Name,
                            Value = featureMeasure.Quality,
                            Group = "features"
                        };
                        result.Nodes.Add(fnode);
                    }
                    var fedge = new GraphEdge()
                    {
                        From = snode.Id,
                        To = fnode.Id,
                        Value =  fnode.Value - service.Slo,
                        Tags = new Dictionary<string, object>() {
                            { "Availability", fnode.Value }
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
        public async Task<IEnumerable<ProductGetListRp>> GetProducts(int customerId)
        {
            var entities = await this._dbContext.Products.Where(c => c.Customer.Id == customerId).ToListAsync();
            return this._mapper.Map<IEnumerable<ProductGetListRp>>(entities);
        }
        public async Task<IEnumerable<ProductGetListRp>> GetProductsWithInformation(int customerId)
        {
            var products = await this._dbContext.Products.Where(c => c.CustomerId == customerId).ToListAsync(); 

            var services = await  this._dbContext.Services
                .Include(c=>c.FeatureMap)
                .Where(c => c.Product.CustomerId == customerId).ToListAsync();

            var features = await  this._dbContext.Features
                .Include(c=>c.Indicators)                
                .Include(c=>c.Squads)
                .Where(c => c.Product.CustomerId == customerId).ToListAsync();

            var squads = await this._dbContext.Squads
                .Include(c=>c.FeatureMaps)
                .Where(c => c.CustomerId == customerId).ToListAsync();
            
            var sources = this._dbContext.Sources.Where(c => c.Product.CustomerId == customerId).ToList();

            foreach (var squad in squads.SelectMany(c=>c.FeatureMaps))
            {
                squad.Feature = features.Single(c => c.Id == squad.FeatureId);                
            }

            foreach (var item in products)
            {
                item.Services = services.Where(c => c.ProductId == item.Id).ToList();
                item.Features = features.Where(c => c.ProductId == item.Id).ToList();
                item.Sources = new SourceCollection(sources.Where(c => c.ProductId == item.Id).ToList());

                foreach (var service in item.Services)
                {
                    foreach (var map in service.FeatureMap)
                    {
                        map.Feature = features.Single(c => c.Id == map.FeatureId);
                        foreach (var indicator in map.Feature.Indicators)
                        {
                            indicator.Source = sources.Single(c => c.Id == indicator.SourceId);
                        }
                        foreach (var target in map.Feature.Squads.ToList())
                        {
                            target.Squad = squads.Single(c => c.Id == target.SquadId);
                        }
                    }
                }                

            }

            List<ProductGetListRp> models = new List<ProductGetListRp>();

            foreach (var item in products)
            {
                var tmp  = this._mapper.Map<ProductGetListRp>(item);
                var agg = new FeatureOwnershipAggregate(squads, item.Features);
                tmp.Ownership = agg.Measure().assigned;
                models.Add(tmp);
            }

            return models;
        }

        #region Daily Reports


        private MultiSeriesGetRp InternalGetDailyServiceSeries(ProductEntity product, DateTime start, DateTime end) {            

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
                debtSerie.Items.Add(new SeriesItemGetRp(item.Date, item.Measure.Debt));                                
            }
            result.Series.Add(debtSerie);            

            return result;
        }
        public async Task<MultiSeriesGetRp> GetDailyServiceSeriesByIdAndGroup(int productId, DatePeriodValue period, string group)
        {
            var product = await this._dbContext.FullLoadProductWithGroupAndSourceItems(productId, group, period.Start, period.End);
            return this.InternalGetDailyServiceSeries(product, period.Start, period.End);
        }
        public async Task<MultiSeriesGetRp> GetDailyServiceSeriesById(int productId, DatePeriodValue period)
        {
            var product = await this._dbContext.FullLoadProductWithSourceItems(productId, period.Start, period.End);
            return this.InternalGetDailyServiceSeries(product, period.Start, period.End);
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
                feature.MeasureQuality();
                featuresExports.Add(new ExportExcelFeatureRp(feature));

                foreach (var indicator in feature.Indicators)
                {                    
                    featuresDetailExports.Add(new ExportExcelFeatureDetailRp( indicator));
                }
            }
            
            var serviceExports = new List<ExportExcelServiceRp>();
            var serviceDetailExports = new List<ExportExcelServiceDetailRp>();
            foreach (var service in product.Services)
            {
                foreach (var map in service.FeatureMap)
                {
                    map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                }
                service.MeasureQuality();
                serviceExports.Add(new ExportExcelServiceRp(service));
             
                foreach (var featureMap in service.FeatureMap)
                {                    
                    serviceDetailExports.Add(new ExportExcelServiceDetailRp(featureMap));
                }               
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var servicesSheet = package.Workbook.Worksheets.Add("Services");
                servicesSheet.Cells.LoadFromCollection(serviceExports, true);
                var servicesDetailSheet = package.Workbook.Worksheets.Add("ServicesDetail");
                servicesDetailSheet.Cells.LoadFromCollection(serviceDetailExports, true);
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

        public async Task<ProductDashboardRp> GetServiceGroupDashboard(int productId,
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
            var temp = new List<ProductDashboardRp.ServiceGroupRp>();            
            
            foreach (var group in product.Services.GroupBy(c=>c.Group))
            {
                var targetGroup = new ProductDashboardRp.ServiceGroupRp
                {
                    Total = group.Count(),
                    Name = group.Key
                };
                foreach (var service in group)
                {                    
                    foreach (var map in service.FeatureMap)
                    {
                        map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                    }
                    var measure = service.MeasureQuality();
                    
                    if (measure.Quality < service.Slo)
                    {
                        targetGroup.Fail += 1;
                        sloFails += 1;
                    }
                }
                temp.Add(targetGroup);                
            }

            result.Groups.Add(new ProductDashboardRp.ServiceGroupRp()
            {
                Name = "All",
                Total = product.Services.Count,
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
                var measure = source.MeasureProportion();                
                
                result.Sources.Add(new SourceGetListRp()
                {
                    Id = source.Id.Value,
                    Availability = measure.Proportion,                    
                    Avatar = source.Avatar,
                    CreatedBy = source.CreatedBy,
                    CreatedOn = source.CreatedOn.Value,
                    GoodDefinition = source.GoodDefinition,
                    TotalDefinition = source.TotalDefinition,
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
                var measure = feature.MeasureQuality();
                tmp.LoadQuality(measure);
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

            foreach (var service in product.Services)
            {
                foreach (var map in service.FeatureMap)
                {
                    map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                }                
                
                var measure = service.MeasureQuality();

                if (measure.Quality < service.Slo)
                {
                    sloFails += 1;
                }

                var tmp = this._mapper.Map<ServiceGetListRp>(service);
                result.Services.Add(tmp);
                result.ServiceMaps[service.Id.Value] = service.FeatureMap.OrderBy(c=>c.Id).Select(c => c.FeatureId).ToList();
                
            }

            result.Services = result.Services.OrderBy(c => c.Quality).ToList();
            result.SLOFail = sloFails;
            result.SLOProportion = QualityUtils.CalculateFailProportion(product.Services.Count, sloFails);
            result.SourceStats = new StatsValue(result.Sources.Select(c => c.Availability));            
            result.FeaturesStats = new StatsValue(result.Features.Select(c => c.Quality));
            result.FeaturesCoverage = QualityUtils.CalculateProportion(product.Features.Count,
                squadsData.Select(c=>c.FeatureId).Distinct().Count());

            result.ServicesStats = new StatsValue(result.Services.Select(c => c.Quality));
            return result;
        }
        #endregion

        #region Exports
        

        public async Task<MemoryStream> ExportItems(int productId, DatePeriodValue period)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sources = await this._dbContext.Sources.Where(c=>c.ProductId == productId).ToListAsync();

                var ids = sources.Select(c => c.Id.Value).Distinct().ToList();
                var sourceItems = await this._dbContext.GetSourceItems(ids, period.Start, period.End);

                var aggregate = new BackupItemsAggregate(sources, sourceItems);
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

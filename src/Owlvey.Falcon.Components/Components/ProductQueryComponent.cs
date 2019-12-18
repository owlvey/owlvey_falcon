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

namespace Owlvey.Falcon.Components
{
    public class ProductQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly FeatureQueryComponent _featureQueryComponent;
        private readonly ServiceQueryComponent _serviceQueryComponent;

        public ProductQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway,
            FeatureQueryComponent featureQueryComponent, ServiceQueryComponent serviceQueryComponent,
            IMapper mapper, IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
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


        public async Task<GraphGetRp> GetGraph(int id, DateTime start, DateTime end)
        {

            GraphGetRp result = new GraphGetRp();

            var product = await this.GetProductById(id);
            result.Name = product.Name;
            result.Id = product.Id;
            result.Avatar = product.Avatar;

            var services = await this._serviceQueryComponent.GetServicesWithAvailability(id, start, end);

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
            foreach (var service in services)
            {
                var snode = new GraphNode
                {
                    Id = string.Format("service_{0}", service.Id),
                    Avatar = service.Avatar,
                    Name = string.Format("{0} [ {1} | {2} ]", service.Name,
                        Math.Round(service.SLO, 2),
                        Math.Round(service.Availability, 2)),
                    Value = service.Availability,
                    Group = "services",
                    Slo = service.SLO,
                    Importance = AvailabilityUtils.MeasureImpact(service.SLO),
                    Budget = service.Availability - (decimal)service.SLO
                };

                
                result.Nodes.Add(snode);

                var features = await this._featureQueryComponent.GetFeaturesByServiceIdWithAvailability(service.Id, start, end);
                               

                foreach (var feature in features)
                {
                    var Id = string.Format("feature_{0}", feature.Id);
                    var fnode = result.Nodes.SingleOrDefault(c => c.Id == Id);
                    if (fnode == null)
                    {
                        fnode = new GraphNode
                        {
                            Id = Id,
                            Avatar = feature.Avatar,
                            Name = feature.Name,
                            Value = feature.Availability,
                            Group = "features"
                        };
                        result.Nodes.Add(fnode);
                    }
                    var fedge = new GraphEdge()
                    {
                        From = snode.Id,
                        To = fnode.Id,
                        Value =  fnode.Value - service.FeatureSlo,
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
        public async Task<IEnumerable<ProductGetListRp>> GetProductsWithServices(int customerId)
        {
            var entities = await this._dbContext.Products.Include(c => c.Services).Where(c => c.Customer.Id == customerId).ToListAsync();
            return this._mapper.Map<IEnumerable<ProductGetListRp>>(entities);
        }

        #region Daily Reports


        public async Task<MultiSeriesGetRp> GetDailyFeaturesSeriesById(int productId, DateTime start, DateTime end) {

            var product = await this._dbContext.Products
                .Include(c => c.Features)
                .ThenInclude(c=>c.Indicators)
                .ThenInclude(c=>c.Source)
                .Where(c => c.Id == productId).SingleAsync();

            foreach (var feature in product.Features)
            {
                foreach (var indicator in feature.Indicators)
                {
                    var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                    indicator.Source.SourceItems = sourceItems;
                }
            }
            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = product.Name,
                Avatar = product.Avatar
            };

            foreach (var feature in product.Features)
            {
                var aggregate = new FeatureDailyAvailabilityAggregate(feature, start, end);
                var (_, series, _) = aggregate.MeasureAvailability();                
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("Feature:{0}", feature.Id),
                    Avatar = feature.Avatar,
                    Items  =  series.Select(c=> this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }
            return result;
        }
        public async Task<MultiSeriesGetRp> GetDailyServiceSeriesByIdAndGroup(int productId, DateTime start, DateTime end, string group)
        {
            var product = await this._dbContext.Products
               .Include(c => c.Services).Where(c => c.Id == productId).SingleAsync();

            product.Services = product.Services.Where(c => String.Equals(c.Group, group,
                StringComparison.InvariantCultureIgnoreCase)).ToList();

            foreach (var service in product.Services)
            {
                var serviceMaps = await this._dbContext.ServiceMaps
                    .Include(c => c.Feature)
                    .ThenInclude(c => c.Indicators).Where(c => c.Service.Id == service.Id).ToListAsync();
                foreach (var map in serviceMaps)
                {
                    var entity = await this._dbContext.Features
                        .Include(c => c.Indicators)
                        .ThenInclude(c => c.Source)
                        .SingleAsync(c => c.Id == map.Feature.Id);

                    foreach (var indicator in entity.Indicators)
                    {
                        var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                        indicator.Source.SourceItems = sourceItems;
                    }
                    map.Feature = entity;
                }
                service.FeatureMap = serviceMaps;

            }

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = product.Name,
                Avatar = product.Avatar
            };

            var aggregator = new ProductAvailabilityAggregate(product, start, end);

            var (_, availability, features) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = product.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in features)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("Service:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }

            return result;
        }
        public async Task<MultiSeriesGetRp> GetDailyServiceSeriesById(int productId, DateTime start, DateTime end)
        {
            var product = await this._dbContext.Products
                .Include(c => c.Services).Where(c => c.Id == productId).SingleAsync();

            foreach (var service in product.Services)
            {
                var serviceMaps = await this._dbContext.ServiceMaps
                    .Include(c => c.Feature)
                    .ThenInclude(c => c.Indicators).Where(c => c.Service.Id == service.Id).ToListAsync();
                foreach (var map in serviceMaps)
                {
                    var entity = await this._dbContext.Features
                        .Include(c => c.Indicators)
                        .ThenInclude(c => c.Source)
                        .SingleAsync(c => c.Id == map.Feature.Id);

                    foreach (var indicator in entity.Indicators)
                    {
                        var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                        indicator.Source.SourceItems = sourceItems;
                    }
                    map.Feature = entity;
                }
                service.FeatureMap = serviceMaps;

            }

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = product.Name,
                Avatar = product.Avatar
            };

            var aggregator = new ProductAvailabilityAggregate(product, start, end);

            var (_, availability, features) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = product.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in features)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("Service:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }

            return result;

        }



        #endregion

        #region Reports
        public async Task<(ProductEntity, MemoryStream)> GetProductExportToExcel(int productId,
         DateTime start, DateTime end)
        {
            var product = await this._dbContext.FullLoadProduct(productId);

            var sourceItems = this._dbContext.GetSourceItemsByProduct(productId, start, end);

            var squadsData = await this._dbContext.SquadFeatures
                .Include(c => c.Squad)
                .Where(c => c.Feature.ProductId == productId).ToListAsync();

            var sourceExports = new List<ExportExcelSourceRp>();

            foreach (var source in product.Sources)
            {
                source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();
                source.MeasureAvailability();
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
                feature.MeasureAvailability();
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
                service.MeasureAvailability();
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
            result.FeatureTotal = await this._dbContext.Features.Where(c => c.ProductId == productId).CountAsync();
            result.FeatureAssigned = product.Services.SelectMany(c => c.FeatureMap.Select(d => d.FeatureId)).Distinct().Count();

            result.SourceTotal = await this._dbContext.Sources.Where(c => c.ProductId == productId).CountAsync();
            result.SourceAssigned = product.Services.SelectMany(
                c => c.FeatureMap.SelectMany(d => d.Feature.Indicators).Select(e => e.SourceId)).Distinct().Count();
                                    
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
                    var agg = new ServiceAvailabilityAggregate(service);                    
                    var (availability, _, _) = agg.MeasureAvailability();
                    if (availability < service.Slo)
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


            var sourceItems = this._dbContext.GetSourceItemsByProduct(productId, start, end);
            var result = new OperationProductDashboardRp();

            foreach (var source in product.Sources)
            {
                source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();
                var agg = new SourceAvailabilityAggregate(source);
                var (ava, total, good) = agg.MeasureAvailability();
                result.Sources.Add(new SourceGetListRp()
                {
                    Id = source.Id.Value,
                    Availability = ava,
                    Total = total,
                    Good = good,
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

                var agg = new FeatureAvailabilityAggregate(feature);
                var tmp = this._mapper.Map<FeatureGetListRp>(feature);
                (tmp.Availability, _, _) = agg.MeasureAvailability();
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
                var agg = new ServiceAvailabilityAggregate(service);
                var tmp = this._mapper.Map<ServiceGetListRp>(service);
                (tmp.Availability, _, _) = agg.MeasureAvailability();
                result.Services.Add(tmp);

                result.ServiceMaps[service.Id.Value] = service.FeatureMap.OrderBy(c=>c.Id).Select(c => c.FeatureId).ToList();

                if (tmp.Availability < service.Slo)
                {
                    sloFails += 1;
                }
            }

            result.Services = result.Services.OrderBy(c => c.Availability).ToList();
            result.SLOFail = sloFails;
            result.SLOProportion = AvailabilityUtils.CalculateFailProportion(product.Services.Count, sloFails);
            result.SourceStats = new StatsValue(result.Sources.Select(c => c.Availability));
            result.SourceTotal = result.Sources.Where(c=>c.Kind == SourceKindEnum.Interaction).Sum(c => c.Total);
            result.FeaturesStats = new StatsValue(result.Features.Select(c => c.Availability));
            result.FeaturesCoverage = AvailabilityUtils.CalculateProportion(product.Features.Count,
                squadsData.Select(c=>c.FeatureId).Distinct().Count());

            result.ServicesStats = new StatsValue(result.Services.Select(c => c.Availability));
            return result;
        }
        #endregion
    }
}

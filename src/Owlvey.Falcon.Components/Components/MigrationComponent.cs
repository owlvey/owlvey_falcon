using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OfficeOpenXml;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class MigrationComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly SquadComponent _squadComponent;
        private readonly ProductComponent _productComponent;
        private readonly ServiceComponent _serviceComponent;
        private readonly SquadQueryComponent _squadQueryComponent;
        private readonly SourceComponent _sourceComponent;
        private readonly FeatureComponent _featureComponent;
        private readonly ServiceMapComponent _serviceMapComponent;
        private readonly IndicatorComponent _indicatorComponent;
        private readonly SourceItemComponent _sourceItemComponent;
        private readonly IncidentComponent _incidentComponent;
        public MigrationComponent(FalconDbContext dbContext,
            ProductComponent productComponent,
            ServiceComponent serviceComponent,
            FeatureComponent featureComponent,
            SourceComponent sourceComponent,
            ServiceMapComponent serviceMapComponent,
            IndicatorComponent indicatorComponent,
            SourceItemComponent sourceItemComponent,
            IncidentComponent incidentComponent,
            IUserIdentityGateway identityService,
            IDateTimeGateway dateTimeGateway, IMapper mapper,
            SquadComponent squadComponent, SquadQueryComponent squadQueryComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._sourceComponent = sourceComponent;
            this._dbContext = dbContext;
            this._squadComponent = squadComponent;
            this._squadQueryComponent = squadQueryComponent;
            this._productComponent = productComponent;
            this._serviceComponent = serviceComponent;
            this._featureComponent = featureComponent;
            this._serviceMapComponent = serviceMapComponent;
            this._indicatorComponent = indicatorComponent;
            this._sourceComponent = sourceComponent;
            this._sourceItemComponent = sourceItemComponent;
            this._incidentComponent = incidentComponent;
        }


        #region exports

        public async Task<List<string>> ImportMetadata(int customerId, Stream input)
        {
            var logs = new List<string>(); 

            var customer = await this._dbContext.Customers.Where(c => c.Id == customerId).SingleAsync();
            var squads = await this._dbContext.Squads.Where(c => c.CustomerId == customerId).ToListAsync();

            using (var package = new ExcelPackage(input))
            {
                var squadSheet = package.Workbook.Worksheets["Squads"];

                for (int row = 2; row <= squadSheet.Dimension.Rows; row++)
                {
                    var name = squadSheet.Cells[row, 1].GetValue<string>();
                    var description = squadSheet.Cells[row, 2].GetValue<string>();
                    var avatar = squadSheet.Cells[row, 3].GetValue<string>();
                    logs.Add(" add update " + name);
                    await this._squadComponent.CreateOrUpdate(customer, name, description, avatar);
                }

                var productSheet = package.Workbook.Worksheets["Products"];

                for (int row = 2; row <= productSheet.Dimension.Rows; row++)
                {
                    var name = productSheet.Cells[row, 1].GetValue<string>();
                    var description = productSheet.Cells[row, 2].GetValue<string>();
                    var avatar = productSheet.Cells[row, 3].GetValue<string>();
                    if (name != null)
                    {
                        await this._productComponent.CreateOrUpdate(customer, name, description, avatar);
                    }
                }

                var serviceSheet = package.Workbook.Worksheets["Services"];

                for (int row = 2; row <= serviceSheet.Dimension.Rows; row++)
                {
                    var product = serviceSheet.Cells[row, 1].GetValue<string>();
                    var name = serviceSheet.Cells[row, 2].GetValue<string>();
                    var description = serviceSheet.Cells[row, 3].GetValue<string>();
                    var slo = serviceSheet.Cells[row, 4].GetValue<decimal>();
                    var avatar = serviceSheet.Cells[row, 5].GetValue<string>();
                    if (product != null && name != null)
                    {
                        await this._serviceComponent.CreateOrUpdate(customer, product, name, description, avatar, slo);
                    }
                }

                var featureSheet = package.Workbook.Worksheets["Features"];

                for (int row = 2; row <= featureSheet.Dimension.Rows; row++)
                {
                    var product = featureSheet.Cells[row, 1].GetValue<string>();
                    var name = featureSheet.Cells[row, 2].GetValue<string>();
                    var description = featureSheet.Cells[row, 3].GetValue<string>();
                    var avatar = featureSheet.Cells[row, 4].GetValue<string>();
                    if (product != null && name != null)
                    {
                        await this._featureComponent.CreateOrUpdate(customer, product, name, description, avatar);
                    }
                }

                var sourceSheet = package.Workbook.Worksheets["Sources"];
                for (int row = 2; row <= sourceSheet.Dimension.Rows; row++)
                {
                    var product = sourceSheet.Cells[row, 1].GetValue<string>();
                    var name = sourceSheet.Cells[row, 2].GetValue<string>();
                    var tags = sourceSheet.Cells[row, 3].GetValue<string>();
                    var good = sourceSheet.Cells[row, 4].GetValue<string>();
                    var total = sourceSheet.Cells[row, 5].GetValue<string>();
                    var avatar = sourceSheet.Cells[row, 6].GetValue<string>();
                    if (product != null && name != null)
                    {
                        await this._sourceComponent.CreateOrUpdate(customer, product, name, tags, avatar, good, total);
                    }

                }

                var serviceMapSheet = package.Workbook.Worksheets["ServicesMap"];
                for (int row = 2; row <= serviceMapSheet.Dimension.Rows; row++)
                {
                    var product = serviceMapSheet.Cells[row, 1].GetValue<string>();
                    var service = serviceMapSheet.Cells[row, 2].GetValue<string>();
                    var feature = serviceMapSheet.Cells[row, 3].GetValue<string>();
                    if (product != null && service != null)
                    {
                        await this._serviceMapComponent.CreateServiceMap(customerId, product, service, feature);
                    }
                }

                var indicatorSheet = package.Workbook.Worksheets["Indicators"];
                for (int row = 2; row <= indicatorSheet.Dimension.Rows; row++)
                {
                    var product = indicatorSheet.Cells[row, 1].GetValue<string>();
                    var source = indicatorSheet.Cells[row, 2].GetValue<string>();
                    var feature = indicatorSheet.Cells[row, 3].GetValue<string>();
                    if (product != null && source != null)
                    {
                        await this._indicatorComponent.Create(customerId, product, source, feature);
                    }
                }

                var sourceItemsSheet = package.Workbook.Worksheets["SourceItems"];

                var sources = await this._dbContext.Sources.Include(c => c.Product).Where(c => c.Product.CustomerId == customerId).ToListAsync();

                for (int row = 2; row <= sourceItemsSheet.Dimension.Rows; row++)
                {
                    var product = sourceItemsSheet.Cells[row, 1].GetValue<string>();
                    var source = sourceItemsSheet.Cells[row, 2].GetValue<string>();
                    var good = sourceItemsSheet.Cells[row, 3].GetValue<int>();
                    var total = sourceItemsSheet.Cells[row, 4].GetValue<int>();
                    var start = DateTime.Parse(sourceItemsSheet.Cells[row, 5].GetValue<string>());
                    var end = DateTime.Parse(sourceItemsSheet.Cells[row, 6].GetValue<string>());
                    if (product != null && source != null)
                    {
                        await this._sourceItemComponent.Create(new SourceItemPostRp()
                        {
                            SourceId = sources.Where(c => c.Name == source && c.Product.Name == product).Single().Id.Value,
                            Start = start,
                            End = end,
                            Good = good,
                            Total = total
                        });
                    }
                }

                var incidentsSheet = package.Workbook.Worksheets["Incidents"];
                var products = await this._dbContext.Products.Where(c => c.CustomerId == customerId).ToListAsync();

                for (int row = 2; row <= incidentsSheet.Dimension.Rows; row++)
                {
                    IncidentMigrationRp a;
                    var product = incidentsSheet.Cells[row, 1].GetValue<string>();
                    var key = incidentsSheet.Cells[row, 2].GetValue<string>();
                    if (!string.IsNullOrWhiteSpace(product) && !string.IsNullOrWhiteSpace(key)) {
                        var title = incidentsSheet.Cells[row, 3].GetValue<string>();
                        var affected = incidentsSheet.Cells[row, 4].GetValue<int>();
                        var ttd = incidentsSheet.Cells[row, 5].GetValue<int>();
                        var tte = incidentsSheet.Cells[row, 6].GetValue<int>();
                        var ttf = incidentsSheet.Cells[row, 7].GetValue<int>();
                        var url = incidentsSheet.Cells[row, 8].GetValue<string>();
                        var end = DateTime.Parse(incidentsSheet.Cells[row, 9].GetValue<string>());

                        var (incident, created) = await this._incidentComponent.Post(new IncidentPostRp()
                        {
                            Key = key,
                            Title = title,
                            ProductId = products.Where(c => c.Name == product).Single().Id.Value
                        });

                        await this._incidentComponent.Put(incident.Id, new IncidentPutRp()
                        {
                            End = end,
                            Title = title,
                            TTD = ttd,
                            TTE = tte,
                            TTF = ttf,
                            URL = url,
                            Affected = affected
                        });
                    }
                }

                var incidentsFeaturesSheet = package.Workbook.Worksheets["IncidentFeatures"];
                var incidents = await  this._dbContext.Incidents.Where(c => c.Product.CustomerId == customerId).ToListAsync();
                var features = await this._dbContext.Features.Where(c => c.Product.CustomerId == customerId).ToListAsync();
                for (int row = 2; row <= incidentsFeaturesSheet.Dimension.Rows; row++) {
                    var product = incidentsFeaturesSheet.Cells[row, 1].GetValue<string>();
                    var key = incidentsFeaturesSheet.Cells[row, 2].GetValue<string>();
                    var feature = incidentsFeaturesSheet.Cells[row, 3].GetValue<string>();
                    await this._incidentComponent.RegisterFeature(
                        incidents.Where(c=>c.Key == key).Single().Id.Value, 
                        features.Where(c=>c.Name == feature).Single().Id.Value);                    
                }
            }
            return logs;
        }
        public async Task<(
            CustomerEntity customer,
            IEnumerable<SquadMigrationRp> squads,
            IEnumerable<MemberMigrateRp> members,
            IEnumerable<ProductMigrationRp> products,
            IEnumerable<ServiceMigrateRp> services,
            IEnumerable<ServiceMapMigrateRp> serviceMaps,
            IEnumerable<FeatureMigrateRp> features,
            IEnumerable<IndicatorMigrateRp> indicators,
            IEnumerable<SourceMigrateRp> sources,
            IEnumerable<SourceItemMigrationRp> items,
            IEnumerable<IncidentMigrationRp> incidents,
            IEnumerable<IncidentFeatureMigrationRp> incidentMaps
            )> Export(int customerId, bool includeItems = false)
        {
            var customer = await this._dbContext.Customers
                .Include(c => c.Products).ThenInclude(c => c.Services).Where(c => c.Id == customerId)                
                .SingleAsync();

            var incidents = await this._dbContext.Incidents.Include(c=>c.FeatureMaps).ThenInclude(c=>c.Feature).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var squads = await this._dbContext.Squads.Include(c => c.Members).ThenInclude(c => c.User).Where(c => c.CustomerId == customerId).ToListAsync();
            var squadLites = this._mapper.Map<IEnumerable<SquadMigrationRp>>(squads);
            var productLites = this._mapper.Map<IEnumerable<ProductMigrationRp>>(customer.Products);
            var serviceLites = this._mapper.Map<IEnumerable<ServiceMigrateRp>>(customer.Products.SelectMany(c => c.Services));
            var memberLites = new List<MemberMigrateRp>();
            foreach (var squad in squads)
            {
                foreach (var item in squad.Members)
                {
                    memberLites.Add(new MemberMigrateRp()
                    {
                        Email = item.User.Email,
                        Squad = squad.Name
                    });
                }
            }

            var features = await this._dbContext.Features.Include(c => c.Product).Include(c => c.ServiceMaps).ThenInclude(c => c.Service).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var featureLites = new List<FeatureMigrateRp>();
            var serviceMapLites = new List<ServiceMapMigrateRp>();

            foreach (var item in features)
            {
                var tmp = this._mapper.Map<FeatureMigrateRp>(item);
                featureLites.Add(tmp);

                foreach (var serv in item.ServiceMaps)
                {
                    serviceMapLites.Add(new ServiceMapMigrateRp()
                    {
                        Product = item.Product.Name,
                        Feature = item.Name,
                        Service = serv.Service.Name
                    });
                }
            }

            var sources = await this._dbContext.Sources.Include(c => c.Product).Include(c => c.Indicators).ThenInclude(c => c.Feature).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var sourceLites = this._mapper.Map<IEnumerable<SourceMigrateRp>>(sources);

            var indicatorLites = new List<IndicatorMigrateRp>();

            foreach (var item in sources)
            {
                foreach (var indicator in item.Indicators)
                {
                    indicatorLites.Add(new IndicatorMigrateRp()
                    {
                        Product = item.Product.Name,
                        Feature = indicator.Feature.Name,
                        Source = item.Name
                    });
                }
            }

            List<SourceItemMigrationRp> items = new List<SourceItemMigrationRp>();
            if (includeItems) {
                var temp = await this._dbContext.SourcesItems.Include(c=>c.Source).Where(c => c.Source.Product.CustomerId == customerId).ToListAsync();
                foreach (var item in temp)
                {
                    var product = customer.Products.Where(c => c.Id == item.Source.ProductId).Single();

                    items.Add(new SourceItemMigrationRp()
                    {
                        Product = product.Name,
                        End = item.End.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                        Start = item.Start.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                        Good = item.Good,
                        Source = item.Source.Name,
                        Total = item.Total
                    });

                }
            }

            List<IncidentMigrationRp> incidentsLite = new List<IncidentMigrationRp>();
            List<IncidentFeatureMigrationRp> incidentFeaturesLite = new List<IncidentFeatureMigrationRp>();
            foreach (var item in incidents)
            {
                incidentsLite.Add(new IncidentMigrationRp() {
                    Product = item.Product.Name,
                    Key = item.Key,
                    Affected = item.Affected,
                    Start = item.Start.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                    End = item.End.ToString("s", System.Globalization.CultureInfo.InvariantCulture),                     
                    Title = item.Title,
                    TTD = item.TTD,
                    TTE = item.TTE,
                    TTF = item.TTF,
                    URL = item.Url
                });
                foreach (var incidentFeature in item.FeatureMaps)
                {
                    incidentFeaturesLite.Add(new IncidentFeatureMigrationRp() {
                         Product = item.Product.Name,
                         Feature = incidentFeature.Feature.Name,
                         IncidentKey = item.Key
                    });
                }
            }
            return (customer, squadLites, memberLites, productLites, serviceLites, serviceMapLites, featureLites, indicatorLites, sourceLites, items, incidentsLite, incidentFeaturesLite);

        }

        public async Task<(CustomerEntity entity, MemoryStream stream)> ExportExcel(int customerId, bool includeData)
        {
            var (customer, squadLites, memberLites, productLites, serviceLites, serviceMapLites, featureLites,
                  indicatorLites, sourceLites, sourceItemsLites, incidentLites, incidentFeatureMap) = await this.Export(customerId, includeData);

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var squadSheet = package.Workbook.Worksheets.Add("Squads");
                squadSheet.Cells.LoadFromCollection(squadLites, true);

                var membersSheet = package.Workbook.Worksheets.Add("Members");
                membersSheet.Cells.LoadFromCollection(memberLites, true);

                var productSheet = package.Workbook.Worksheets.Add("Products");
                productSheet.Cells.LoadFromCollection(productLites, true);

                var servicesSheet = package.Workbook.Worksheets.Add("Services");
                servicesSheet.Cells.LoadFromCollection(serviceLites, true);

                var servicesMapSheet = package.Workbook.Worksheets.Add("ServicesMap");
                servicesMapSheet.Cells.LoadFromCollection(serviceMapLites, true);

                var featuresSheet = package.Workbook.Worksheets.Add("Features");
                featuresSheet.Cells.LoadFromCollection(featureLites, true);

                var indicatorsSheet = package.Workbook.Worksheets.Add("Indicators");
                indicatorsSheet.Cells.LoadFromCollection(indicatorLites, true);

                var sourcesSheet = package.Workbook.Worksheets.Add("Sources");
                sourcesSheet.Cells.LoadFromCollection(sourceLites, true);

                var sourceItemsSheet = package.Workbook.Worksheets.Add("SourceItems");
                sourceItemsSheet.Cells.LoadFromCollection(sourceItemsLites, true);

                var incidentSheet = package.Workbook.Worksheets.Add("Incidents");
                incidentSheet.Cells.LoadFromCollection(incidentLites, true);

                var incidentFeaturesSheet = package.Workbook.Worksheets.Add("IncidentFeatures");
                incidentFeaturesSheet.Cells.LoadFromCollection(incidentFeatureMap, true);

                package.Save();
            }
            stream.Position = 0;
            return (customer, stream);
        }
       

        #endregion

    }
}

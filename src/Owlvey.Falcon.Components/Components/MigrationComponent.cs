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
using Newtonsoft.Json;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core;

namespace Owlvey.Falcon.Components
{
    public class MigrationComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly CustomerComponent _customerComponent;
        private readonly SquadComponent _squadComponent;
        private readonly ProductComponent _productComponent;
        private readonly ProductQueryComponent _productQueryComponent;
        private readonly ServiceComponent _serviceComponent;
        private readonly SquadQueryComponent _squadQueryComponent;
        private readonly SourceComponent _sourceComponent;
        private readonly FeatureComponent _featureComponent;
        private readonly ServiceMapComponent _serviceMapComponent;
        private readonly IndicatorComponent _indicatorComponent;
        private readonly SourceItemComponent _sourceItemComponent;
        private readonly IncidentComponent _incidentComponent;
        private readonly UserComponent _userComponent;
        private readonly FeatureQueryComponent _featureQueryComponent;
        


        public MigrationComponent(FalconDbContext dbContext,
            CustomerComponent customerComponent,
            ProductComponent productComponent,
            ServiceComponent serviceComponent,
            FeatureComponent featureComponent,
            SourceComponent sourceComponent,
            ServiceMapComponent serviceMapComponent,
            IndicatorComponent indicatorComponent,
            SourceItemComponent sourceItemComponent,
            IncidentComponent incidentComponent,
            UserComponent userComponent,
            ProductQueryComponent productQueryComponent,
            IUserIdentityGateway identityService,
            IDateTimeGateway dateTimeGateway, IMapper mapper,
            FeatureQueryComponent featureQueryComponent,
            SquadComponent squadComponent, SquadQueryComponent squadQueryComponent,
            ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._customerComponent = customerComponent;
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
            this._userComponent = userComponent;
            this._productQueryComponent = productQueryComponent;
            this._featureQueryComponent = featureQueryComponent;
        }


        #region exports


        public async Task<IEnumerable<SquadFeatureMigrationRp>> ExportSquadMap(int customerId) {

            var result = new List<SquadFeatureMigrationRp>();

            var squads = await this._dbContext.Squads.Include(c=>c.FeatureMaps)
                .ThenInclude(c=>c.Feature)
                .ThenInclude(c=>c.Product)
                .Where(c => c.CustomerId == customerId).ToListAsync();

            foreach (var squad in squads)
            {
                foreach (var map in squad.FeatureMaps)
                {
                    var temp = new SquadFeatureMigrationRp()
                    {
                        Product = map.Feature.Product.Name,
                        Squad = squad.Name,
                        Feature = map.Feature.Name
                    };

                    if (!result.Contains( temp , new SquadFeatureMigrationRp.Comparer())) {
                        result.Add(temp);
                    }                    
                }                                           
            }
            return result;
        }

        public async Task<IEnumerable<AnchorMigrationRp>> ExportAnchors(int customerId)
        {
            var result = new List<AnchorMigrationRp>();
            var anchors = await this._dbContext.Anchors.Include(c=>c.Product).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            foreach (var anchor in anchors)
            {                
                result.Add(new AnchorMigrationRp()
                {
                     Product = anchor.Product.Name,
                     Name = anchor.Name,
                     Target = anchor.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture) 
                });                
            }
            return result;
        }

        public async Task<IEnumerable<IndicatorMigrateRp>> ExportIndicators(int customerId) {
            var result = new List<IndicatorMigrateRp>();

            var indicators = await this._dbContext.Indicators
                .Include(c=>c.Source)
                .Include(c=>c.Feature).ThenInclude(c=>c.Product)
                .Where(c => c.Feature.Product.CustomerId == customerId).ToListAsync();

            foreach (var item in indicators.OrderBy(c=>c.Id))
            {
                result.Add(new IndicatorMigrateRp() {
                     Product = item.Feature.Product.Name,
                     Feature = item.Feature.Name,
                     Source = item.Source.Name
                });
            }

            return result;

        }

        public async Task<IEnumerable<ServiceMapMigrateRp>> ExportServiceMap(int customerId)
        {
            var result = new List<ServiceMapMigrateRp>();

            var maps = await this._dbContext.ServiceMaps
                .Include(c=>c.Service).ThenInclude(c=>c.Product)
                .Include(c=>c.Feature)
                .Where(c => c.Service.Product.CustomerId == customerId).ToListAsync();

            foreach (var item in maps.OrderBy(c=>c.Id))
            {
                result.Add(new ServiceMapMigrateRp() {
                     Product = item.Service.Product.Name,
                     Service = item.Service.Name,
                     Feature = item.Feature.Name
                });
            }
            return result;
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
            IEnumerable<IncidentFeatureMigrationRp> incidentMaps,
            IEnumerable<SquadFeatureMigrationRp> squadMaps,
            IEnumerable<AnchorMigrationRp> anchors
            )> Export(int customerId, bool includeItems = false)
        {
            var customer = await this._dbContext.Customers
                .Include(c => c.Products).ThenInclude(c => c.Services).Where(c => c.Id == customerId)
                .SingleAsync();

            var incidents = await this._dbContext.Incidents.Include(c => c.FeatureMaps).ThenInclude(c => c.Feature).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var squads = await this._dbContext.Squads.Include(c => c.Members).ThenInclude(c => c.User).Where(c => c.CustomerId == customerId).ToListAsync();
            var squadLites = this._mapper.Map<IEnumerable<SquadMigrationRp>>(squads);
            var productLites = this._mapper.Map<IEnumerable<ProductMigrationRp>>(customer.Products);

            var serviceLites = this._mapper.Map<IEnumerable<ServiceMigrateRp>>(
                    customer.Products.SelectMany(c => c.Services).OrderBy(c=>c.Id).ToList()
                );

            var memberLites = new List<MemberMigrateRp>();
            foreach (var squad in squads)
            {
                foreach (var item in squad.Members)
                {
                    memberLites.Add(new MemberMigrateRp()
                    {
                        Email = item.User.Email,
                        Squad = squad.Name,
                        Avatar = item.User.Avatar,
                        Name = item.User.Name,
                        SlackMember = item.User.SlackMember
                    });
                }
            }

            var features = await this._dbContext.Features
                .Include(c => c.Product)
                .Include(c => c.ServiceMaps).ThenInclude(c => c.Service)
                .Where(c => c.Product.CustomerId == customerId).ToListAsync();

            var featureLites = new List<FeatureMigrateRp>();
            var serviceMapLites = await this.ExportServiceMap(customerId);

            foreach (var item in features)
            {
                var tmp = this._mapper.Map<FeatureMigrateRp>(item);
                featureLites.Add(tmp);             
            }

            var sources = await this._dbContext.Sources
                .Include(c => c.Product)
                .Include(c => c.Indicators)                
                .ThenInclude(c => c.Feature)
                .Where(c => c.Product.CustomerId == customerId)
                .ToListAsync();

            var sourceLites = this._mapper.Map<IEnumerable<SourceMigrateRp>>(sources);

            var indicatorLites = await this.ExportIndicators(customerId);             

            List<SourceItemMigrationRp> items = new List<SourceItemMigrationRp>();
            if (includeItems)
            {
                var temp = await this._dbContext.SourcesItems
                    .Include(c => c.Source)                    
                    .Where(c => c.Source.Product.CustomerId == customerId).ToListAsync();
                foreach (var item in temp)
                {
                    var product = customer.Products.Where(c => c.Id == item.Source.ProductId).Single();
                    var map = new SourceItemMigrationRp()
                    {
                        Product = product.Name,
                        Target = item.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture),                        
                        Source = item.Source.Name,                        
                        Proportion = item.Measure
                    };
                    
                    map.Total = item.Total.GetValueOrDefault();
                    map.Good = item.Good.GetValueOrDefault();                        
                    
                    items.Add(map);                    
                }
            }

            List<IncidentMigrationRp> incidentsLite = new List<IncidentMigrationRp>();
            List<IncidentFeatureMigrationRp> incidentFeaturesLite = new List<IncidentFeatureMigrationRp>();
            foreach (var item in incidents)
            {
                incidentsLite.Add(new IncidentMigrationRp()
                {
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
                    incidentFeaturesLite.Add(new IncidentFeatureMigrationRp()
                    {
                        Product = item.Product.Name,
                        Feature = incidentFeature.Feature.Name,
                        IncidentKey = item.Key
                    });
                }
            }

            var squadFeatureLite = await this.ExportSquadMap(customerId);
            var anchors = await this.ExportAnchors(customerId);

            return (customer, squadLites, memberLites, productLites, serviceLites, serviceMapLites, featureLites, indicatorLites, sourceLites, items, incidentsLite,
                        incidentFeaturesLite,
                        squadFeatureLite,
                        anchors);
        }

        public async Task<(CustomerEntity entity, MemoryStream stream)> ExportExcel(int customerId, bool includeData)
        {
            var (customer, squadLites, memberLites, productLites, serviceLites, serviceMapLites, featureLites,
                  indicatorLites, sourceLites, sourceItemsLites, incidentLites, incidentFeatureMap,
                  squadFeatureMap, anchors) = await this.Export(customerId, includeData);

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

                var squadFeaturesSheet = package.Workbook.Worksheets.Add("SquadFeatures");
                squadFeaturesSheet.Cells.LoadFromCollection(squadFeatureMap, true);

                var syncSheet = package.Workbook.Worksheets.Add("Anchors");
                syncSheet.Cells.LoadFromCollection(anchors);

                package.Save();
            }
            stream.Position = 0;
            return (customer, stream);
        }


        #endregion
        #region Import

        private async Task ImportMembers(int customerId, ExcelWorksheet membersSource) {

            for (int row = 2; row <= membersSource.Dimension.Rows; row++)
            {
                var email = membersSource.Cells[row, 1].GetValue<string>();
                var name = membersSource.Cells[row, 2].GetValue<string>();
                var squad = membersSource.Cells[row, 3].GetValue<string>();
                var avatar = membersSource.Cells[row, 4].GetValue<string>();
                var slackmember = membersSource.Cells[row, 5].GetValue<string>();
                var user = await this._userComponent.CreateOrUpdate(email, name, avatar, slackmember);
                await this._squadComponent.RegisterMember(customerId, squad, user.Id);
            }
        }

        private async Task ImportAnchors(int customerId, ExcelWorksheet source)
        {

            for (int row = 2; row <= source.Dimension.Rows; row++)
            {
                var product = source.Cells[row, 1].GetValue<string>();
                var name = source.Cells[row, 2].GetValue<string>();                
                var target = DateTime.Parse(source.Cells[row, 3].GetValue<string>());
                var pproduct = await this._productQueryComponent.GetProductByName(customerId, product);
                await this._productComponent.CreateOrUpdateAnchor(pproduct.Id, name, target);                
            }
        }

        private async Task ImportSquadFeatures(int customerId, ExcelWorksheet source) {
            for (int row = 2; row <= source.Dimension.Rows; row++)
            {
                var product = source.Cells[row, 1].GetValue<string>();                
                var squad = source.Cells[row, 2].GetValue<string>();
                var feature = source.Cells[row, 3].GetValue<string>();
                var psquad = await this._squadQueryComponent.GetSquadByName(customerId, squad);
                var pproduct = await this._productQueryComponent.GetProductByName(customerId, product);
                var pfeature = await this._featureQueryComponent.GetFeatureByName(pproduct.Id, feature);

                await this._featureComponent.RegisterSquad(new SquadFeaturePostRp() {
                     SquadId = psquad.Id,
                     FeatureId =  pfeature.Id
                });                                
            }
        }
        public async Task ImportSource(CustomerEntity customer, ExcelWorksheet source) {            
            for (int row = 2; row <= source.Dimension.Rows; row++)
            {
                var product = source.Cells[row, 1].GetValue<string>();
                var name = source.Cells[row, 2].GetValue<string>();
                var tags = source.Cells[row, 3].GetValue<string>();
                var good = source.Cells[row, 4].GetValue<string>();
                var total = source.Cells[row, 5].GetValue<string>();
                var avatar = source.Cells[row, 6].GetValue<string>();
                var description = source.Cells[row, 7].GetValue<string>();
                var kind = source.Cells[row, 8].GetValue<string>();
                var group = source.Cells[row, 9].GetValue<string>();
                var percentile = source.Cells[row, 10].GetValue<decimal>();
                if (product != null && name != null)
                {
                    await this._sourceComponent.CreateOrUpdate(customer,
                        product, name, tags, avatar, good, total, description, kind, group, percentile);
                }
            }
        }
        

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
                    var leaders = squadSheet.Cells[row, 4].GetValue<string>();
                    logs.Add(" add update " + name);
                    await this._squadComponent.CreateOrUpdate(customer, name, description, avatar, leaders);
                }

                var memberSheet = package.Workbook.Worksheets["Members"];
                await this.ImportMembers(customerId, memberSheet);

                var productSheet = package.Workbook.Worksheets["Products"];

                for (int row = 2; row <= productSheet.Dimension.Rows; row++)
                {
                    var name = productSheet.Cells[row, 1].GetValue<string>();
                    var description = productSheet.Cells[row, 2].GetValue<string>();
                    var avatar = productSheet.Cells[row, 3].GetValue<string>();
                    var leaders = productSheet.Cells[row, 4].GetValue<string>();
                    if (name != null)
                    {
                        await this._productComponent.CreateOrUpdate(customer, name, description, avatar, leaders);
                    }
                }

                var serviceSheet = package.Workbook.Worksheets["Services"];

                for (int row = 2; row <= serviceSheet.Dimension.Rows; row++)
                {
                    var product = serviceSheet.Cells[row, 1].GetValue<string>();
                    var name = serviceSheet.Cells[row, 2].GetValue<string>();
                    var description = serviceSheet.Cells[row, 3].GetValue<string>();
                    var availabilitySlo = serviceSheet.Cells[row, 4].GetValue<decimal>();
                    var latencySlo = serviceSheet.Cells[row, 5].GetValue<decimal>();
                    var experienceSlo = serviceSheet.Cells[row, 6].GetValue<decimal>();
                    var avatar = serviceSheet.Cells[row, 7].GetValue<string>();
                    var leaders = serviceSheet.Cells[row, 8].GetValue<string>();
                    var aggregation = serviceSheet.Cells[row, 9].GetValue<string>();
                    var group = serviceSheet.Cells[row, 10].GetValue<string>();
                    if (product != null && name != null)
                    {
                        await this._serviceComponent.CreateOrUpdate(customer, 
                            product, name, description, 
                            avatar, availabilitySlo, latencySlo, experienceSlo, leaders, aggregation, group);
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
                await this.ImportSource(customer, sourceSheet);                

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
                    var feature = indicatorSheet.Cells[row, 2].GetValue<string>();
                    var source = indicatorSheet.Cells[row, 3].GetValue<string>();                    
                    if (product != null && source != null)
                    {
                        await this._indicatorComponent.Create(customerId, product, source, feature);
                    }
                }

                var sourceItemsSheet = package.Workbook.Worksheets["SourceItems"];

                var sources = await this._dbContext.Sources.Include(c => c.Product).Where(c => c.Product.CustomerId == customerId).ToListAsync();

                var sourceItems = new List<(SourceEntity, SourceItemInteractionPostRp)>();
                for (int row = 2; row <= sourceItemsSheet.Dimension.Rows; row++)
                {
                    var product = sourceItemsSheet.Cells[row, 1].GetValue<string>();
                    var source = sourceItemsSheet.Cells[row, 2].GetValue<string>();
                    var good = sourceItemsSheet.Cells[row, 3].GetValue<int>();
                    var total = sourceItemsSheet.Cells[row, 4].GetValue<int>();
                    var target = DateTime.Parse(sourceItemsSheet.Cells[row, 5].GetValue<string>());                    
                    var clues = sourceItemsSheet.Cells[row, 6].GetValue<string>();
                    if (product != null && source != null)
                    {
                        var tmp = sources.Where(c => c.Name == source && c.Product.Name == product).Single();

                        var targetClues = new Dictionary<string, decimal>();
                        if (!string.IsNullOrWhiteSpace(clues)) {
                            targetClues = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(clues);
                        }

                        sourceItems.Add((tmp, new SourceItemInteractionPostRp()
                        {
                            SourceId = tmp.Id.Value,
                            Start = target,
                            End = target,
                            Good = good,
                            Total = total                            
                        }));                        
                    }
                }

                var groups = sourceItems.GroupBy(c => c.Item1, new SourceEntityComparer());
                foreach (var item in groups)
                {
                    await this._sourceItemComponent.BulkInsert(item.Key, item.Select(c=>c.Item2).ToList());
                }


                var incidentsSheet = package.Workbook.Worksheets["Incidents"];
                var products = await this._dbContext.Products.Where(c => c.CustomerId == customerId).ToListAsync();

                for (int row = 2; row <= incidentsSheet.Dimension.Rows; row++)
                {                    
                    var product = incidentsSheet.Cells[row, 1].GetValue<string>();
                    var key = incidentsSheet.Cells[row, 2].GetValue<string>();
                    if (!string.IsNullOrWhiteSpace(product) && !string.IsNullOrWhiteSpace(key))
                    {
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
                var incidents = await this._dbContext.Incidents.Where(c => c.Product.CustomerId == customerId).ToListAsync();
                var features = await this._dbContext.Features.Where(c => c.Product.CustomerId == customerId).ToListAsync();
                for (int row = 2; row <= incidentsFeaturesSheet.Dimension.Rows; row++)
                {
                    var product = incidentsFeaturesSheet.Cells[row, 1].GetValue<string>();
                    var key = incidentsFeaturesSheet.Cells[row, 2].GetValue<string>();
                    var feature = incidentsFeaturesSheet.Cells[row, 3].GetValue<string>();
                    await this._incidentComponent.RegisterFeature(
                        incidents.Where(c => c.Key == key).Single().Id.Value,
                        features.Where(c => c.Name == feature).Single().Id.Value);
                }

                var squadFeaturesSheet = package.Workbook.Worksheets["SquadFeatures"];
                await this.ImportSquadFeatures(customerId, squadFeaturesSheet);

                var anchorsSheet = package.Workbook.Worksheets["Anchors"];
                await this.ImportAnchors(customerId, anchorsSheet);
            }
            return logs;
        }

        #endregion

        #region backups


        public async Task<IEnumerable<string>> Restore(MemoryStream input) {

            var logs = new List<string>();            

            var previous = await this._dbContext.Customers.ToListAsync();
            foreach (var item in previous)
            {
                await this._customerComponent.DeleteCustomer(item.Id.Value);
            }            

            using (var package = new ExcelPackage(input))
            {

                var usersSheet = package.Workbook.Worksheets["Users"];
                for (int row = 2; row <= usersSheet.Dimension.Rows; row++)
                {
                    var name = usersSheet.Cells[row, 1].GetValue<string>();
                    var avatar = usersSheet.Cells[row, 2].GetValue<string>();
                    var email = usersSheet.Cells[row, 3].GetValue<string>();
                    var slack = usersSheet.Cells[row, 3].GetValue<string>();
                    await this._userComponent.CreateOrUpdate(email, name, avatar, slack);
                }

                var users = await this._dbContext.Users.ToListAsync();

                var organizationSheet = package.Workbook.Worksheets["Organizations"];
                for (int row = 2; row <= organizationSheet.Dimension.Rows; row++)
                {
                    var name = organizationSheet.Cells[row, 1].GetValue<string>();
                    var avatar = organizationSheet.Cells[row, 2].GetValue<string>();
                    var leaders = organizationSheet.Cells[row, 3].GetValue<string>();                                        
                    await this._customerComponent.CreateOrUpdate(name, avatar, leaders);
                }
                               
                var organizations = await this._dbContext.Customers.ToListAsync();

                var productsSheet = package.Workbook.Worksheets["Products"];
                for (int row = 2; row <= productsSheet.Dimension.Rows; row++)
                {
                    var organization = productsSheet.Cells[row, 1].GetValue<string>();
                    var name = productsSheet.Cells[row, 2].GetValue<string>();
                    var description = productsSheet.Cells[row, 3].GetValue<string>();
                    var leaders = productsSheet.Cells[row, 4].GetValue<string>();
                    var avatar = productsSheet.Cells[row, 5].GetValue<string>();
                    await this._productComponent.CreateOrUpdate(organizations.Single(c=>c.Name == organization),
                        name, description, avatar, leaders);
                }
                
                var products = await this._dbContext.Products.ToListAsync();
                foreach (var item in products)
                {
                    item.Customer = organizations.Single(c => c.Id == item.CustomerId);
                }

                var anchorSheet = package.Workbook.Worksheets["Anchors"];
                for (int row = 2; row <= anchorSheet.Dimension.Rows; row++)
                {
                    var organization = anchorSheet.Cells[row, 1].GetValue<string>();
                    var product = anchorSheet.Cells[row, 2].GetValue<string>();
                    var name = anchorSheet.Cells[row, 3].GetValue<string>();
                    var target = DateTime.Parse(anchorSheet.Cells[row, 4].GetValue<string>());
                    await this._productComponent.CreateOrUpdateAnchor(
                        products.Single(c=>c.Name == product && c.Customer.Name == organization).Id.Value,
                        name, target);
                }

                var squadSheet = package.Workbook.Worksheets["Squads"];
                for (int row = 2; row <= squadSheet.Dimension.Rows; row++)
                {
                    var organization = squadSheet.Cells[row, 1].GetValue<string>();
                    var name = squadSheet.Cells[row, 2].GetValue<string>();
                    var avatar = squadSheet.Cells[row, 3].GetValue<string>();
                    var description = squadSheet.Cells[row, 4].GetValue<string>();
                    var leaders = squadSheet.Cells[row, 5].GetValue<string>();
                    await this._squadComponent.CreateOrUpdate(organizations.Single(c => c.Name == organization),
                        name, description, avatar, leaders);                    
                }

                var squads = await this._dbContext.Squads.ToListAsync();
                foreach (var item in squads)
                {
                    item.Customer = organizations.Single(c => c.Id == item.CustomerId);
                }

                var membersSheet = package.Workbook.Worksheets["Members"];
                for (int row = 2; row <= membersSheet.Dimension.Rows; row++)
                {
                    var organization = membersSheet.Cells[row, 1].GetValue<string>();
                    var email = membersSheet.Cells[row, 2].GetValue<string>();
                    var squad  = membersSheet.Cells[row, 3].GetValue<string>();                    
                    await this._squadComponent.RegisterMember(
                          squads.Where(c=>c.Customer.Name == organization && c.Name == squad).Single().Id.Value,
                          users.Single(c=>c.Email == email).Id.Value);
                }

                var servicesSheet = package.Workbook.Worksheets["Services"];
                for (int row = 2; row <= servicesSheet.Dimension.Rows; row++)
                {
                    var organization = servicesSheet.Cells[row, 1].GetValue<string>();
                    var product = servicesSheet.Cells[row, 2].GetValue<string>();
                    var name = servicesSheet.Cells[row, 3].GetValue<string>();
                    var group = servicesSheet.Cells[row, 4].GetValue<string>();
                    var availabilitySlo = servicesSheet.Cells[row, 5].GetValue<decimal>();
                    var latencySlo = servicesSheet.Cells[row, 6].GetValue<decimal>();
                    var experienceSlo = servicesSheet.Cells[row, 7].GetValue<decimal>();
                    var description = servicesSheet.Cells[row, 8].GetValue<string>();
                    var avatar = servicesSheet.Cells[row, 9].GetValue<string>();
                    var leaders = servicesSheet.Cells[row, 10].GetValue<string>();
                    var aggregation = servicesSheet.Cells[row, 11].GetValue<string>();
                    await this._serviceComponent.CreateOrUpdate(
                        organizations.Single(c=>c.Name == organization),product, name,
                        description, avatar, availabilitySlo, latencySlo, experienceSlo, leaders, aggregation, group);
                }

                var services = await this._dbContext.Services.ToListAsync();
                foreach (var item in services)
                {
                    item.Product = products.Single(c => c.Id == item.ProductId);
                }
                
                var featuresSheet = package.Workbook.Worksheets["Features"];

                for (int row = 2; row <= featuresSheet.Dimension.Rows; row++)
                {
                    var organization = featuresSheet.Cells[row, 1].GetValue<string>();
                    var product = featuresSheet.Cells[row, 2].GetValue<string>();
                    var name = featuresSheet.Cells[row, 3].GetValue<string>();
                    var avatar = featuresSheet.Cells[row, 4].GetValue<string>();
                    var description = featuresSheet.Cells[row, 5].GetValue<string>();
                    await this._featureComponent.CreateOrUpdate(
                        organizations.Single(c => c.Name == organization), product, name, description, avatar);
                }

                var features = await this._dbContext.Features.ToListAsync();
                foreach (var item in features)
                {
                    item.Product = products.Single(c => c.Id == item.ProductId);
                }

                var serviceMapSheet = package.Workbook.Worksheets["ServiceMaps"];

                for (int row = 2; row <= serviceMapSheet.Dimension.Rows; row++)
                {
                    var organization = serviceMapSheet.Cells[row, 1].GetValue<string>();
                    var product = serviceMapSheet.Cells[row, 2].GetValue<string>();
                    var service = serviceMapSheet.Cells[row, 3].GetValue<string>();
                    var feature = serviceMapSheet.Cells[row, 4].GetValue<string>();
                    var description = serviceMapSheet.Cells[row, 5].GetValue<string>();
                    await this._serviceMapComponent.CreateServiceMap(
                        organizations.Single(c => c.Name == organization).Id.Value,
                        product, service, feature);
                }

                var sourcesSheet = package.Workbook.Worksheets["Sources"];

                for (int row = 2; row <= sourcesSheet.Dimension.Rows; row++)
                {
                    var organization = sourcesSheet.Cells[row, 1].GetValue<string>();
                    var product = sourcesSheet.Cells[row, 2].GetValue<string>();
                    var name = sourcesSheet.Cells[row, 3].GetValue<string>();
                    var avatar = sourcesSheet.Cells[row, 4].GetValue<string>();
                    var description = sourcesSheet.Cells[row, 5].GetValue<string>();
                    var group = sourcesSheet.Cells[row, 6].GetValue<string>();
                    var good = sourcesSheet.Cells[row, 7].GetValue<string>();
                    var total = sourcesSheet.Cells[row, 8].GetValue<string>();
                    var kind = sourcesSheet.Cells[row, 9].GetValue<string>();
                    var percentile = sourcesSheet.Cells[row, 10].GetValue<decimal>();
                    await this._sourceComponent.CreateOrUpdate(
                        organizations.Single(c => c.Name == organization), product, name, string.Empty, avatar, good, total, description, kind, group, percentile);                        
                }

                var sources = await this._dbContext.Sources.ToListAsync();
                foreach (var item in sources)
                {
                    item.Product = products.Single(c => c.Id == item.ProductId);
                }

                var indicatorsSheet = package.Workbook.Worksheets["Indicators"];

                for (int row = 2; row <= indicatorsSheet.Dimension.Rows; row++)
                {
                    var organization = indicatorsSheet.Cells[row, 1].GetValue<string>();
                    var product = indicatorsSheet.Cells[row, 2].GetValue<string>();
                    var feature = indicatorsSheet.Cells[row, 3].GetValue<string>();
                    var source = indicatorsSheet.Cells[row, 4].GetValue<string>();                    
                    await this._indicatorComponent.Create(
                          features.Single(c => c.Name == feature && c.Product.Name == product && c.Product.Customer.Name == organization).Id.Value,
                          sources.Single(c => c.Name == source && c.Product.Name == product && c.Product.Customer.Name == organization).Id.Value
                        );
                }

                var squadFeaturesSheet = package.Workbook.Worksheets["SquadFeatures"];

                for (int row = 2; row <= squadFeaturesSheet.Dimension.Rows; row++)
                {
                    var organization = squadFeaturesSheet.Cells[row, 1].GetValue<string>();
                    var product = squadFeaturesSheet.Cells[row, 2].GetValue<string>();
                    var feature = squadFeaturesSheet.Cells[row, 3].GetValue<string>();
                    var squad = squadFeaturesSheet.Cells[row, 4].GetValue<string>();

                    var featureId = features.Where(c => c.Name == feature && c.Product.Name == product && c.Product.Customer.Name == organization).Single().Id.Value;
                    var squadId = squads.Where(c => c.Name == squad && c.Customer.Name == organization).Single().Id.Value;
                    await this._featureComponent.RegisterSquad(new SquadFeaturePostRp() {
                        FeatureId = featureId,
                        SquadId = squadId,
                    });
                }

                var sourceItemsSheet = package.Workbook.Worksheets["SourceItems"];

                var sourceItems= new List<(SourceEntity, SourceItemPostRp)>();
                

                for (int row = 2; row <= sourceItemsSheet.Dimension.Rows; row++)
                {
                    var organization = sourceItemsSheet.Cells[row, 1].GetValue<string>();
                    var product = sourceItemsSheet.Cells[row, 2].GetValue<string>();
                    var source = sourceItemsSheet.Cells[row, 3].GetValue<string>();
                    var good = sourceItemsSheet.Cells[row, 4].GetValue<int>();
                    var total = sourceItemsSheet.Cells[row, 5].GetValue<int>();
                    var target = DateTime.Parse(sourceItemsSheet.Cells[row, 6].GetValue<string>());
                    var proportion = sourceItemsSheet.Cells[row, 7].GetValue<decimal?>();

                    var sourceTarget = sources.Single(c => c.Name == source && c.Product.Name == product && c.Product.Customer.Name == organization);

                    if (sourceTarget.Kind == SourceKindEnum.Interaction)
                    {
                        sourceItems.Add((sourceTarget, new SourceItemInteractionPostRp()
                        {
                            SourceId = sourceTarget.Id.Value,
                            Start = target,
                            End = target,
                            Good = good,
                            Total = total
                        }));
                    }
                    else {
                        sourceItems.Add((sourceTarget, new SourceItemProportionPostRp()
                        {
                            SourceId = sourceTarget.Id.Value,
                            Start = target,
                            End = target,
                            Proportion = proportion.HasValue ? proportion.Value : QualityUtils.CalculateProportion(total, good),                            
                        })); ;
                    }                    
                }

                var groups = sourceItems.GroupBy(c => c.Item1, new SourceEntityComparer());
                foreach (var item in groups)
                {
                    await this._sourceItemComponent.BulkInsert(item.Key, item.Select(c => c.Item2).ToList());
                }
            }

            return logs;
        }
        
        public async Task<MemoryStream> Backup(bool includeData)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var users = await this._dbContext.Users.ToListAsync();
                var customers = await this._dbContext.Customers
                    .Include(c => c.Products).ThenInclude(c => c.Anchors)
                    .Include(c => c.Squads).ThenInclude(d => d.Members)
                    .ToListAsync();

                var services = await this._dbContext.Services.Include(c => c.FeatureMap).ToListAsync();
                var features = await this._dbContext.Features
                    .Include(c => c.Indicators)
                    .Include(c => c.Squads)
                    .ToListAsync();
                var sources = await this._dbContext.Sources.ToListAsync();

                var sourceItems = new List<SourceItemEntity>();
                if (includeData)
                {
                    sourceItems = await this._dbContext.SourcesItems.ToListAsync();
                }

                var aggregate = new BackupAggregate(users, customers, services, features, sources, sourceItems);
                var model = aggregate.Execute();

                var usersSheet = package.Workbook.Worksheets.Add("Users");
                usersSheet.Cells.LoadFromCollection(model.Users, true);

                var customerSheet = package.Workbook.Worksheets.Add("Organizations");
                customerSheet.Cells.LoadFromCollection(model.Organizations, true);

                var productsSheet = package.Workbook.Worksheets.Add("Products");
                productsSheet.Cells.LoadFromCollection(model.Products, true);

                var anchorsSheet = package.Workbook.Worksheets.Add("Anchors");
                anchorsSheet.Cells.LoadFromCollection(model.Anchors, true);                

                var squadsSheet = package.Workbook.Worksheets.Add("Squads");
                squadsSheet.Cells.LoadFromCollection(model.Squads, true);

                var membersSheet = package.Workbook.Worksheets.Add("Members");
                membersSheet.Cells.LoadFromCollection(model.Members, true);

                var servicesSheet = package.Workbook.Worksheets.Add("Services");
                servicesSheet.Cells.LoadFromCollection(model.Services, true);

                var serviceMapsSheet = package.Workbook.Worksheets.Add("ServiceMaps");
                serviceMapsSheet.Cells.LoadFromCollection(model.ServiceMaps, true);

                var featuresSheet = package.Workbook.Worksheets.Add("Features");
                featuresSheet.Cells.LoadFromCollection(model.Features, true);

                var indicatorsSheet = package.Workbook.Worksheets.Add("Indicators");
                indicatorsSheet.Cells.LoadFromCollection(model.Indicators, true);

                var squadFeaturesSheet = package.Workbook.Worksheets.Add("SquadFeatures");
                squadFeaturesSheet.Cells.LoadFromCollection(model.SquadFeatures, true);

                var sourcesSheet = package.Workbook.Worksheets.Add("Sources");
                sourcesSheet.Cells.LoadFromCollection(model.Sources, true);

                var sourceItemsSheet = package.Workbook.Worksheets.Add("SourceItems");
                sourceItemsSheet.Cells.LoadFromCollection(model.SourceItems, true);

                package.Save();
            }
            stream.Position = 0;
            return stream;
        }

        #endregion



    }
}

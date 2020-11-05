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
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Core.Models.Migrate;
using Owlvey.Falcon.Builders;
using Microsoft.EntityFrameworkCore.Internal;

namespace Owlvey.Falcon.Components
{
    public class MigrationComponent : BaseComponent
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SecurityThreatEntity, SecurityThreatModel>();
            cfg.CreateMap<SecurityRiskEntity, SecurityRiskModel>()
                .ForMember(c => c.Source, d => d.MapFrom(c => c.Source.Name))
                .ForMember(c => c.Product, d => d.MapFrom(c => c.Source.Product.Name))
                .ForMember(c => c.Organization, d => d.MapFrom(c => c.Source.Product.Customer.Name));
            cfg.CreateMap<ReliabilityThreatEntity, ReliabilityThreatModel>();
            cfg.CreateMap<ReliabilityRiskEntity, ReliabilityRiskModel>()
                .ForMember(c => c.Source, d => d.MapFrom(c => c.Source.Name))
                .ForMember(c => c.Product, d => d.MapFrom(c => c.Source.Product.Name))
                .ForMember(c => c.Organization, d => d.MapFrom(c => c.Source.Product.Customer.Name));
        }
        private readonly FalconDbContext _dbContext;
        private readonly CustomerComponent _customerComponent;
        private readonly SquadComponent _squadComponent;
        private readonly ProductComponent _productComponent;
        private readonly ProductQueryComponent _productQueryComponent;
        private readonly JourneyComponent _journeyComponent;
        private readonly SquadQueryComponent _squadQueryComponent;
        private readonly SourceComponent _sourceComponent;
        private readonly FeatureComponent _featureComponent;
        private readonly JourneyMapComponent _journeyMapComponent;
        private readonly IndicatorComponent _indicatorComponent;
        private readonly SourceItemComponent _sourceItemComponent;
        private readonly IncidentComponent _incidentComponent;
        private readonly UserComponent _userComponent;
        private readonly FeatureQueryComponent _featureQueryComponent;
        private readonly SecurityRiskComponent _securityRiskComponent;
        private readonly ReliabilityRiskComponent _reliabilityRiskComponent;

        private const string SHEET_SecurityThreatsName = "SecurityThreats";
        private const string SHEET_SecurityRisks = "SecurityRisks";
        private const string SHEET_ReliabilityThreats = "ReliabilityThreats";
        private const string SHEET_ReliabilityRisks  = "ReliabilityRisks";
        


        public MigrationComponent(FalconDbContext dbContext,
            CustomerComponent customerComponent,
            ProductComponent productComponent,
            JourneyComponent journeyComponent,
            FeatureComponent featureComponent,
            SourceComponent sourceComponent,
            JourneyMapComponent journeyMapComponent,
            IndicatorComponent indicatorComponent,
            SourceItemComponent sourceItemComponent,
            IncidentComponent incidentComponent,
            UserComponent userComponent,
            ProductQueryComponent productQueryComponent,
            IUserIdentityGateway identityGateway,
            IDateTimeGateway dateTimeGateway, IMapper mapper,
            FeatureQueryComponent featureQueryComponent,
            SecurityRiskComponent securityRiskComponent,
            ReliabilityRiskComponent reliabilityRiskComponent,
            SquadComponent squadComponent, SquadQueryComponent squadQueryComponent,
            ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._customerComponent = customerComponent;
            this._sourceComponent = sourceComponent;
            this._dbContext = dbContext;
            this._squadComponent = squadComponent;
            this._squadQueryComponent = squadQueryComponent;
            this._productComponent = productComponent;
            this._journeyComponent = journeyComponent;
            this._featureComponent = featureComponent;
            this._journeyMapComponent = journeyMapComponent;
            this._indicatorComponent = indicatorComponent;
            this._sourceComponent = sourceComponent;
            this._sourceItemComponent = sourceItemComponent;
            this._incidentComponent = incidentComponent;
            this._userComponent = userComponent;
            this._productQueryComponent = productQueryComponent;
            this._featureQueryComponent = featureQueryComponent;
            this._securityRiskComponent = securityRiskComponent;
            this._reliabilityRiskComponent = reliabilityRiskComponent;
        }


        #region exports


        public async Task<IEnumerable<SquadFeatureMigrationRp>> ExportSquadMap(int customerId)
        {

            var result = new List<SquadFeatureMigrationRp>();

            var squads = await this._dbContext.Squads.Include(c => c.FeatureMaps)
                .ThenInclude(c => c.Feature)
                .ThenInclude(c => c.Product)
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

                    if (!result.Contains(temp, new SquadFeatureMigrationRp.Comparer()))
                    {
                        result.Add(temp);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<AnchorMigrationRp>> ExportAnchors(int customerId)
        {
            var result = new List<AnchorMigrationRp>();
            var anchors = await this._dbContext.Anchors.Include(c => c.Product).Where(c => c.Product.CustomerId == customerId).ToListAsync();
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

        public async Task<IEnumerable<IndicatorMigrateRp>> ExportIndicators(int customerId)
        {
            var result = new List<IndicatorMigrateRp>();

            var indicators = await this._dbContext.Indicators
                .Include(c => c.Source)
                .Include(c => c.Feature).ThenInclude(c => c.Product)
                .Where(c => c.Feature.Product.CustomerId == customerId).ToListAsync();

            foreach (var item in indicators.OrderBy(c => c.Id))
            {
                result.Add(new IndicatorMigrateRp()
                {
                    Product = item.Feature.Product.Name,
                    Feature = item.Feature.Name,
                    Source = item.Source.Name
                });
            }

            return result;

        }

        public async Task<IEnumerable<JourneyMapMigrateRp>> ExportJourneyMap(int customerId)
        {
            var result = new List<JourneyMapMigrateRp>();

            var maps = await this._dbContext.JourneyMaps
                .Include(c => c.Journey).ThenInclude(c => c.Product)
                .Include(c => c.Feature)
                .Where(c => c.Journey.Product.CustomerId == customerId).ToListAsync();

            foreach (var item in maps.OrderBy(c => c.Id))
            {
                result.Add(new JourneyMapMigrateRp()
                {
                    Product = item.Journey.Product.Name,
                    Journey = item.Journey.Name,
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
            IEnumerable<JourneyMigrateRp> journeys,
            IEnumerable<JourneyMapMigrateRp> journeyMaps,
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
                .Include(c => c.Products).ThenInclude(c => c.Journeys).Where(c => c.Id == customerId)
                .SingleAsync();

            var incidents = await this._dbContext.Incidents.Include(c => c.FeatureMaps).ThenInclude(c => c.Feature).Where(c => c.Product.CustomerId == customerId).ToListAsync();
            var squads = await this._dbContext.Squads.Include(c => c.Members).ThenInclude(c => c.User).Where(c => c.CustomerId == customerId).ToListAsync();
            var squadLites = this._mapper.Map<IEnumerable<SquadMigrationRp>>(squads);
            var productLites = this._mapper.Map<IEnumerable<ProductMigrationRp>>(customer.Products);

            var journeyLites = this._mapper.Map<IEnumerable<JourneyMigrateRp>>(
                    customer.Products.SelectMany(c => c.Journeys).OrderBy(c => c.Id).ToList()
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
                .Include(c => c.JourneyMaps).ThenInclude(c => c.Journey)
                .Where(c => c.Product.CustomerId == customerId).ToListAsync();

            var featureLites = new List<FeatureMigrateRp>();
            var journeyMapLites = await this.ExportJourneyMap(customerId);

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

            return (customer, squadLites, memberLites, productLites, journeyLites, journeyMapLites, featureLites, indicatorLites, sourceLites, items, incidentsLite,
                        incidentFeaturesLite,
                        squadFeatureLite,
                        anchors);
        }



        #endregion
        #region Import

        private async Task ImportMembers(int customerId, ExcelWorksheet membersSource)
        {

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

        private async Task ImportSquadFeatures(int customerId, ExcelWorksheet source)
        {
            for (int row = 2; row <= source.Dimension.Rows; row++)
            {
                var product = source.Cells[row, 1].GetValue<string>();
                var squad = source.Cells[row, 2].GetValue<string>();
                var feature = source.Cells[row, 3].GetValue<string>();
                var psquad = await this._squadQueryComponent.GetSquadByName(customerId, squad);
                var pproduct = await this._productQueryComponent.GetProductByName(customerId, product);
                var pfeature = await this._featureQueryComponent.GetFeatureByName(pproduct.Id, feature);

                await this._featureComponent.RegisterSquad(new SquadFeaturePostRp()
                {
                    SquadId = psquad.Id,
                    FeatureId = pfeature.Id
                });
            }
        }
        public async Task ImportSource(CustomerEntity customer, ExcelWorksheet source)
        {
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
                    //TODO: Work
                    await this._sourceComponent.CreateOrUpdate(customer,
                        product, name, tags, avatar,
                        new DefinitionValue(good, total),
                        new DefinitionValue(good, total),
                        new DefinitionValue(good, total),
                        description, percentile);
                }
            }
        }


        #endregion

        #region backups

        public async Task ClearData(){
            var previous = await this._dbContext.Customers.ToListAsync();
            foreach (var item in previous)
            {
                await this._customerComponent.DeleteCustomer(item.Id.Value);
            }
            var previous_security_threats = await this._securityRiskComponent.GetThreats();
            foreach (var item in previous_security_threats)
            {
                await this._securityRiskComponent.DeleteThreat(item.Id);
            }
            var previous_reliability_threats = await this._reliabilityRiskComponent.GetThreats();
            foreach (var item in previous_reliability_threats)
            {
                await this._reliabilityRiskComponent.DeleteThreat(item.Id);
            }
        }
        public async Task<IEnumerable<string>> Restore(MemoryStream input)
        {
            var logs = new List<string>();
            var createdBy = this._identityGateway.GetIdentity();
            var createdOn = this._datetimeGateway.GetCurrentDateTime();

            await this.ClearData();           

            using (var package = new ExcelPackage(input))
            {
                var securityThreatSheet = package.Workbook.Worksheets[SHEET_SecurityThreatsName];
                var securityThreats = SecurityThreatModel.Build(new SheetRowAdapter(securityThreatSheet));
                foreach (var item in securityThreats)
                {
                    var temp = await this._securityRiskComponent.CreateThreat(new SecurityThreatPostRp()
                    {
                        Name = item.Name
                    });
                    await this._securityRiskComponent.UpdateThreat(temp.Id, item);
                }

                var reliabilityThreatSheet = package.Workbook.Worksheets[SHEET_ReliabilityThreats];
                var reliabilityThreats = ReliabilityThreatModel.Build(new SheetRowAdapter(reliabilityThreatSheet));
                foreach (var item in reliabilityThreats)
                {
                    var temp = await this._reliabilityRiskComponent.CreateThreat( new ReliabilityThreatPostRp() { 
                         Name = item.Name
                    } );
                    await this._reliabilityRiskComponent.UpdateThreat(temp.Id, item);
                }

                var usersSheet = package.Workbook.Worksheets["Users"];
                for (int row = 2; row <= usersSheet.Dimension.Rows; row++)
                {
                    var name = usersSheet.Cells[row, 1].GetValue<string>();
                    var avatar = usersSheet.Cells[row, 2].GetValue<string>();
                    var email = usersSheet.Cells[row, 3].GetValue<string>();
                    var slack = usersSheet.Cells[row, 4].GetValue<string>();
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
                    await this._productComponent.CreateOrUpdate(organizations.Single(c => c.Name == organization),
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
                        products.Single(c => c.Name == product && c.Customer.Name == organization).Id.Value,
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
                    var squad = membersSheet.Cells[row, 3].GetValue<string>();
                    await this._squadComponent.RegisterMember(
                          squads.Where(c => c.Customer.Name == organization && c.Name == squad).Single().Id.Value,
                          users.Single(c => c.Email == email).Id.Value);
                }

                var journeysSheet = package.Workbook.Worksheets["Journeys"];
                for (int row = 2; row <= journeysSheet.Dimension.Rows; row++)
                {
                    var organization = journeysSheet.Cells[row, 1].GetValue<string>();
                    var product = journeysSheet.Cells[row, 2].GetValue<string>();
                    var name = journeysSheet.Cells[row, 3].GetValue<string>();
                    var group = journeysSheet.Cells[row, 4].GetValue<string>();
                    var availabilitySlo = journeysSheet.Cells[row, 5].GetValue<decimal>();
                    var latencySlo = journeysSheet.Cells[row, 6].GetValue<decimal>();
                    var experienceSlo = journeysSheet.Cells[row, 7].GetValue<decimal>();
                    var description = journeysSheet.Cells[row, 8].GetValue<string>();
                    var avatar = journeysSheet.Cells[row, 9].GetValue<string>();
                    var leaders = journeysSheet.Cells[row, 10].GetValue<string>();
                    var slaValue = new SLAValue(
                        journeysSheet.Cells[row, 11].GetValue<decimal>(),
                        journeysSheet.Cells[row, 12].GetValue<decimal>()
                    );

                    await this._journeyComponent.CreateOrUpdate(
                        organizations.Single(c => c.Name == organization), product, name,
                        description, avatar, availabilitySlo, latencySlo, experienceSlo,
                        slaValue,
                        leaders, group);
                }

                var journeys = await this._dbContext.Journeys.ToListAsync();
                foreach (var item in journeys)
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

                var jouneryMapSheet = package.Workbook.Worksheets["JourneyMaps"];

                for (int row = 2; row <= jouneryMapSheet.Dimension.Rows; row++)
                {
                    var organization = jouneryMapSheet.Cells[row, 1].GetValue<string>();
                    var product = jouneryMapSheet.Cells[row, 2].GetValue<string>();
                    var journey = jouneryMapSheet.Cells[row, 3].GetValue<string>();
                    var feature = jouneryMapSheet.Cells[row, 4].GetValue<string>();
                    var description = jouneryMapSheet.Cells[row, 5].GetValue<string>();
                    await this._journeyMapComponent.CreateMap(
                        organizations.Single(c => c.Name == organization).Id.Value,
                        product, journey, feature);
                }

                var sourcesSheet = package.Workbook.Worksheets["Sources"];

                var sourcesInstances = SourceModel.Build(organizations, createdOn, createdBy,
                    new SheetRowAdapter(sourcesSheet));

                await this._dbContext.Sources.AddRangeAsync(sourcesInstances);
                await this._dbContext.SaveChangesAsync();

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
                    await this._featureComponent.RegisterSquad(new SquadFeaturePostRp()
                    {
                        FeatureId = featureId,
                        SquadId = squadId,
                    });
                }


                var risksSources = await this._dbContext.Sources.Include(c => c.Product).ThenInclude(c => c.Customer)
                    .ToListAsync();

                var securityRiskSheet = package.Workbook.Worksheets[SHEET_SecurityRisks];
                var securityRisks = SecurityRiskModel.Build(new SheetRowAdapter(securityRiskSheet), risksSources);
                foreach (var item in securityRisks)
                {
                    var temp = await this._securityRiskComponent.Create(item.Item1);
                    await this._securityRiskComponent.UpdateRisk(temp.Id, item.Item2);
                }

                var reliabilityRiskSheet = package.Workbook.Worksheets[SHEET_ReliabilityRisks];
                var reliabilityRisks = ReliabilityRiskModel.Build(new SheetRowAdapter(reliabilityRiskSheet), risksSources);
                foreach (var item in reliabilityRisks)
                {
                    var temp = await this._reliabilityRiskComponent.Create(item.Item1);
                    await this._reliabilityRiskComponent.UpdateRisk(temp.Id, item.Item2);
                }

                var sourceItemsSheet = package.Workbook.Worksheets["SourceItems"];
                var sourceItems = SourceItemModel.Build(organizations, createdOn, createdBy, new SheetRowAdapter(sourceItemsSheet));
                await this._sourceItemComponent.BulkInsert(sourceItems);
            }

            return logs;
        }

        public async Task<MemoryStream> Backup(bool includeData)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var model = await SystemModelBuilder.Build(this._mapper, this._dbContext, includeData);

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

                var journeysSheet = package.Workbook.Worksheets.Add("Journeys");
                journeysSheet.Cells.LoadFromCollection(model.Journeys, true);

                var journeyMapsSheet = package.Workbook.Worksheets.Add("JourneyMaps");
                journeyMapsSheet.Cells.LoadFromCollection(model.JourneyMaps, true);

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

                var securityTHreatsSheet = package.Workbook.Worksheets.Add(SHEET_SecurityThreatsName);
                securityTHreatsSheet.Cells.LoadFromCollection(model.SecurityThreats, true);

                var securityRisksSheet = package.Workbook.Worksheets.Add(SHEET_SecurityRisks);
                securityRisksSheet.Cells.LoadFromCollection(model.SecurityRisks, true);

                var reliabilityThreatsSheet = package.Workbook.Worksheets.Add(SHEET_ReliabilityThreats);
                reliabilityThreatsSheet.Cells.LoadFromCollection(model.ReliabilityThreats, true);

                var reliabilityRisksSheet = package.Workbook.Worksheets.Add(SHEET_ReliabilityRisks);
                reliabilityRisksSheet.Cells.LoadFromCollection(model.ReliabilityRisks, true);

                package.Save();
            }
            stream.Position = 0;
            return stream;
        }

        #endregion



    }
}

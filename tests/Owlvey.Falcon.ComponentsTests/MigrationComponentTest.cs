using KellermanSoftware.CompareNetObjects;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Owlvey.Falcon.Components;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Owlvey.Falcon.Models;
using System;

namespace Owlvey.Falcon.ComponentsTests
{
    public class MigrationComponentTest
    {        
        [Fact]
        public async Task ExporImport()
        {            
            
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var userComponent = container.GetInstance<UserComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var squadComponent = container.GetInstance<SquadComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();
            var productComponent = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test", Default=true
            });

            var user1 = await userComponent.CreateUser(new Models.UserPostRp() { Email = "test1@test.com" });
            var user2 = await userComponent.CreateUser(new Models.UserPostRp() { Email = "test2@test.com" });

            var squads = await squadQueryComponent.GetSquads(result.Id);

            foreach (var item in squads)
            {
                await squadComponent.RegisterMember(item.Id, user1.Id);
                await squadComponent.RegisterMember(item.Id, user2.Id);
            }

            var stream = await productQueryComponent.ExportItems(result.Id, OwlveyCalendar.year2019);

            stream.Position = 0;

            await productComponent.ImportsItems(result.Id, stream);
        }
                
        [Fact]
        public async Task Backup(){
            #region   Components

            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponet = container.GetInstance<CustomerQueryComponent>();
            var productComponent = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();
            var journeyComponent = container.GetInstance<JourneyQueryComponent>();
            var featureComponent = container.GetInstance<FeatureQueryComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();
            var securityComponent = container.GetInstance<SecurityRiskComponent>();
            var reliabilityComponent = container.GetInstance<ReliabilityRiskComponent>();

            #endregion
            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test",  Default = true   
            });
            var product = await productComponent.CreateProduct(new Models.ProductPostRp() { Name = "test", CustomerId = result.Id });
            var SecurityThreat = await securityComponent.CreateThreat(new Models.SecurityThreatPostRp() { Name= "test" } );
            var source = await sourceComponent.Create(new Models.SourcePostRp() {
                 Name = "test", ProductId = product.Id
            });
            var SecurityRisk = await securityComponent.Create(new Models.SecurityRiskPost()
            {
                 SourceId = source.Id,  Name = SecurityThreat.Name
            });


            var reliabilityThreat = await reliabilityComponent.CreateThreat(new Models.ReliabilityThreatPostRp()
            { Name = "test threat" });

            var reliabilityRisk = await reliabilityComponent.Create(new Models.ReliabilityRiskPostRp()
            {
                 Name = "test risk", SourceId = source.Id
            });

            var stream = await migrationComponent.Backup(true);

            

            
        }
        
        [Fact]
        public async Task BackupRestore() {

            #region   Components

            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponet = container.GetInstance<CustomerQueryComponent>();
            var productComponent = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();
            var journeyComponent = container.GetInstance<JourneyQueryComponent>();
            var featureComponent = container.GetInstance<FeatureQueryComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();
            var securityComponent = container.GetInstance<SecurityRiskComponent>();
            var reliabilityComponent = container.GetInstance<ReliabilityRiskComponent>();

            #endregion
            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test",
                Default = true
            });
            var product = await productComponent.CreateProduct(new Models.ProductPostRp() { Name = "test", CustomerId = result.Id });
            
            var source = await sourceComponent.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product.Id
            });

            var securityThreats =  await securityComponent.CreateDefault();
            var SecurityRisk = await securityComponent.Create(new Models.SecurityRiskPost()
            {
                SourceId = source.Id,
                Name = "test risk"
            });
            var reliabiltyThreats =  await reliabilityComponent.CreateDefault();
            
            var reliabilityRisk = await reliabilityComponent.Create(new Models.ReliabilityRiskPostRp()
            {
                Name = "reliability risk",
                SourceId = source.Id
            });

            var stream = await migrationComponent.Backup(true);

            await migrationComponent.ClearData();

            Assert.Empty(await customerQueryComponet.GetCustomers());            
            Assert.Empty(await securityComponent.GetThreats());            
            Assert.Empty(await reliabilityComponent.GetThreats());            

            await migrationComponent.Restore(stream);

            var customer_target = await customerQueryComponet.GetCustomerByName("test");

            Assert.NotNull(customer_target);

            var products = await productQueryComponent.GetProducts(customer_target.Id);                       

            Assert.NotEmpty(products);

            foreach (var item in products)
            {
                var sources = await sourceComponent.GetByProductId(item.Id);
                Assert.NotEmpty(sources);

                var anchors = await productQueryComponent.GetAnchors(item.Id);
                Assert.NotEmpty(anchors);
            }

            var sourceItems = await sourceItemComponent.GetAll();
            
            Assert.NotEmpty(sourceItems);
            
            var squads = await squadQueryComponent.GetSquads(customer_target.Id);

            Assert.NotEmpty(squads);

            var journeys = await journeyComponent.GetListByProductId(customer_target.Id);
            Assert.NotEmpty(journeys);

            foreach (var item in journeys)
            {
                var journey_detail = await journeyComponent.GetJourneyById(item.Id);                
                Assert.NotEmpty(journey_detail.Features);
            }
            
            var features = await featureComponent.GetFeatures(customer_target.Id);
            Assert.NotEmpty(features);            

            var securityRisks = await securityComponent.GetRisks(null);
            Assert.NotEmpty(securityRisks);

            var reliabilityThreats = await reliabilityComponent.GetThreats();
            Assert.NotEmpty(reliabilityThreats);


            var reliabilityRisks = await reliabilityComponent.GetRisks(null);
            Assert.NotEmpty(reliabilityRisks);

            
            
            ComparisonConfig config = new ComparisonConfig();            
            config.IgnoreProperty((SecurityThreatGetRp c)=>c.Id);
            config.IgnoreProperty((ReliabilityThreatGetRp c)=>c.Id);
            config.MaxDifferences = 5;
            var ThreatsCompare = new CompareLogic(config);        
               
            var targets = await securityComponent.GetThreats();
            foreach( var item in securityThreats){                
                var target = targets.Where(e=>e.Name == item.Name).Single();
                var temp = ThreatsCompare.Compare(item,  target);
                if (!temp.AreEqual)
                {
                    throw new ApplicationException(target.Name + " " + temp.DifferencesString);   
                }                
            }

            var reliabilityThreatsRestored = await reliabilityComponent.GetThreats();
            foreach(var item in reliabiltyThreats){
                var target = reliabilityThreatsRestored.Where(e=>e.Name == item.Name).Single();
                var temp = ThreatsCompare.Compare(item,  target);
                if (!temp.AreEqual)
                {
                    throw new ApplicationException(target.Name + " " + temp.DifferencesString);   
                }                
            } 
        }
    }
}
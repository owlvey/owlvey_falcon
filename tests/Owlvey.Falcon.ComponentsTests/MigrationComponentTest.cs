using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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

            var (customer, stream) = await migrationComponent.ExportExcel(result.Id, false);

            stream.Position = 0;

            await migrationComponent.ImportMetadata(customer.Id.Value, stream);
        }


        [Fact]
        public async Task ExporImportEmpty()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var userComponent = container.GetInstance<UserComponent>();            
            
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var squadComponent = container.GetInstance<SquadComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();
            var dbcontext = container.GetInstance<FalconDbContext>();


            var target = new Core.Entities.CustomerEntity() {
                Name = "target_test", Avatar = "target",
                CreatedBy = "test",
                ModifiedBy = "test",
                ModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now };
            dbcontext.Customers.Add(target);
            await dbcontext.SaveChangesAsync();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test", Default= true
            });

            var user1 = await userComponent.CreateUser(new Models.UserPostRp() { Email = "test1@test.com" });
            var user2 = await userComponent.CreateUser(new Models.UserPostRp() { Email = "test2@test.com" });

            var squads = await squadQueryComponent.GetSquads(result.Id);

            foreach (var item in squads)
            {
                await squadComponent.RegisterMember(item.Id, user1.Id);
                await squadComponent.RegisterMember(item.Id, user2.Id);
            }

            var (_, stream) = await migrationComponent.ExportExcel(result.Id, false);

            stream.Position = 0;

            await migrationComponent.ImportMetadata(target.Id.Value, stream);


            var squadsResult = await squadQueryComponent.GetSquads(target.Id.Value);

            Assert.NotEmpty(squadsResult);

            foreach (var item in squadsResult)
            {
                var squadResult = await squadQueryComponent.GetSquadById(item.Id);
                Assert.NotEmpty(squadResult.Members);                
            }

        }

        public async Task ImportFromFileData() {
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
            });
            
            var bytes = File.ReadAllBytes("C:/Users/gcval/Downloads/SBP-data.xlsx");
            var stream = new MemoryStream(bytes);
            stream.Position = 0;
            await migrationComponent.ImportMetadata(result.Id, stream);
        }

        [Fact]
        public async Task BackupRestore() {
            
            #region   Components

            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponet = container.GetInstance<CustomerQueryComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();
            var serviceComponent = container.GetInstance<ServiceQueryComponent>();
            var featureComponent = container.GetInstance<FeatureQueryComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();

            #endregion

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test",  Default = true   
            });            

            var stream = await migrationComponent.Backup(true);

            var customers = await customerQueryComponet.GetCustomers();

            foreach (var item in customers)
            {
                await customerComponet.DeleteCustomer(item.Id);
            }

            customers = await customerQueryComponet.GetCustomers();

            Assert.Empty(customers);

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

            var services = await serviceComponent.GetServices(customer_target.Id);
            Assert.NotEmpty(services);

            foreach (var item in services)
            {
                var service_detail = await serviceComponent.GetServiceById(item.Id);                
                Assert.NotEmpty(service_detail.Features);
            }
            
            var features = await featureComponent.GetFeatures(customer_target.Id);
            Assert.NotEmpty(features);      
            

        }

        [Fact]
        public async Task ExporImportData()
        {

            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var productComponent = container.GetInstance<ProductQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test", Default=true
            });


            var products = await productComponent.GetProducts(result.Id);

            foreach (var item in products)
            {
                var sources = await sourceComponent.GetByProductId(item.Id);

                foreach (var source in sources)
                {
                    var sourceItems = await sourceItemComponent.GetBySource(source.Id);
                    foreach (var sour in sourceItems)
                    {   
                        await sourceItemComponent.Update(sour.Id, sour.Measure,
                            OwlveyCalendar.January201903);
                    }
                }
            }

            var (customer, stream) = await migrationComponent.ExportExcel(result.Id, true);

            stream.Position = 0;

            await migrationComponent.ImportMetadata(customer.Id.Value, stream);
            
            products = await productComponent.GetProducts(result.Id);

            foreach (var item in products)
            {
                var sources = await sourceComponent.GetByProductId(item.Id);

                foreach (var source in sources)
                {
                    var sourceItems = await sourceItemComponent.GetBySource(source.Id);
                    foreach (var sour in sourceItems)
                    {
                        Assert.Equal(OwlveyCalendar.January201903, sour.Target);                        
                    }
                }
            }
        }
    }
}
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
                Name = "test"
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


            var target = new Core.Entities.CustomerEntity() { Name = "target_test", Avatar = "target" };
            dbcontext.Customers.Add(target);
            await dbcontext.SaveChangesAsync();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
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
                Name = "test"
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
                        await sourceItemComponent.Update(sour.Id, sour.Total, sour.Good, OwlveyCalendar.January201903, OwlveyCalendar.January201905);
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
                        Assert.Equal(OwlveyCalendar.January201903, sour.Start);
                        Assert.Equal(OwlveyCalendar.January201905, sour.End);                        
                    }
                }
            }
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.ComponentsTests.Mocks;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SourceComponentTest
    {

        [Fact]
        public async Task SourceIdempotenceTest()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var component = container.GetInstance<SourceComponent>();
            var result = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });
            var result2 = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });
            Assert.Equal(result.Id, result2.Id);
        }


        [Fact]
        public async Task SourceCreation()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();

            var result = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product                 
            });

            Assert.Equal("test", result.Name);
        }

        [Fact]
        public async Task SourceMaintenanceSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();            

            await component.Create(new Models.SourcePostRp() {
                 Name ="test", ProductId = product
            });

            var target = await component.GetByName(product, "test");
            Assert.NotNull(target);
            Assert.NotNull(target.Avatar);
            target = await component.GetById(target.Id);
            Assert.NotNull(target);
            Assert.NotNull(target.Avatar);
        }

        [Fact]
        public async Task SourceDeleteSuccess() {

            var container = ComponentTestFactory.BuildContainer();
            var dbcontext = container.GetInstance<FalconDbContext>();
            var source = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();
            var securityRiskComponent = container.GetInstance<SecurityRiskComponent>();
            var reliabilityRiskComponent = container.GetInstance<ReliabilityRiskComponent>();
            var result = await ComponentTestFactory.BuildCustomerWithSquad(container, 
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);
                        
            var securityRisk = await securityRiskComponent.Create(new SecurityRiskPost()
            {   SourceId = result.sourceId, 
                Name =  MockUtils.GenerateRandomName()
            });

            var reliabilityRisk = await reliabilityRiskComponent.Create(new ReliabilityRiskPostRp()
            {
                SourceId = result.sourceId, 
                Name = "test reliabilty"
            });
            await source.Delete(result.sourceId);

            var securityRisks = await securityRiskComponent.GetRisks(result.sourceId);
            Assert.Empty(securityRisks);

            var reliabilityRisks = await reliabilityRiskComponent.GetRisks(result.sourceId);
            Assert.Empty(reliabilityRisks);

            var sources = await source.GetById(result.sourceId);
            Assert.Null(sources);

            var items = await sourceItemComponent.GetBySource(result.sourceId);
            Assert.Empty(items);

            var expected = dbcontext.SourcesItems.Where(c => c.Source.ProductId == result.productId).ToList();
            Assert.Empty(expected);

            var indicators = dbcontext.Indicators.Where(c => c.FeatureId == result.featureId).ToList();
            Assert.Empty(indicators);
        }


        [Fact]
        public async Task SourceQueryAvailability()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();

            await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });
            var testSource = await component.GetByName(product, "test");

            var item = await sourceItemComponent.CreateAvailabilityItem(new SourceItemAvailabilityPostRp() {
                 SourceId = testSource.Id,  Total = 1000, Good = 800,
                 Start = OwlveyCalendar.January201904,
                 End = OwlveyCalendar.January201906
            });

            var result = await component.GetByProductIdWithAvailability(product, OwlveyCalendar.January201903, OwlveyCalendar.January201907);
            Assert.NotEmpty(result.Items);

            result = await component.GetByProductIdWithAvailability(product, OwlveyCalendar.January201905, OwlveyCalendar.January201907);
            Assert.NotEmpty(result.Items);

        }

        [Fact]
        public async void SourceListSuccess() {
            var container = ComponentTestFactory.BuildContainer();

            var data = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();
            var componentItems = container.GetInstance<SourceItemComponent>();

            var source = await component.Create(new SourcePostRp() { Name = "testSource", ProductId = data.product });

            await componentItems.CreateAvailabilityItem(new SourceItemAvailabilityPostRp() {
                SourceId = source.Id, Start = OwlveyCalendar.StartJanuary2019, 
                End = OwlveyCalendar.EndJanuary2019,
                Good = 800,  Total = 1000
            });

            var items = await componentItems.GetAvailabilityItems(source.Id, OwlveyCalendar.january2019);

            Assert.Equal(0.788m, items.First().Measure);
            Assert.Equal(33,  items.First().Total);
            Assert.Equal(26,  items.First().Good);

            var result = await component.GetByIdWithDetail(source.Id, OwlveyCalendar.january2019);
            Assert.Equal(1023, result.Quality.Total);
        }

        [Fact]
        public async void SourcesByProduct() {
            //GetByProductIdWithAvailability
            var container = ComponentTestFactory.BuildContainer();

            var data = await ComponentTestFactory.BuildCustomerWithSquad(container, OwlveyCalendar.StartJanuary2017, OwlveyCalendar.EndDecember2019);

            var sourceComponent = container.GetInstance<SourceComponent>();

            var result = await sourceComponent.GetByProductIdWithAvailability(data.productId, OwlveyCalendar.StartJanuary2017, OwlveyCalendar.EndDecember2019);

            Assert.Single(result.Items);
        }

      

    }
}

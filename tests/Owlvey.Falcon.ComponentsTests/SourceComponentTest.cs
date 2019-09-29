using System;
using System.Linq;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
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
            var result = await ComponentTestFactory.BuildCustomerWithSquad(container, 
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);

            await source.Delete(result.sourceId);
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

            var item = await sourceItemComponent.Create(new Models.SourceItemPostRp() {
                 SourceId = testSource.Id,  Total = 1000, Good = 800,
                 Start = OwlveyCalendar.January201904,
                 End = OwlveyCalendar.January201906
            });

            var result = await component.GetByProductIdWithAvailability(product, OwlveyCalendar.January201903, OwlveyCalendar.January201907);
            Assert.NotEmpty(result);

            result = await component.GetByProductIdWithAvailability(product, OwlveyCalendar.January201905, OwlveyCalendar.January201907);
            Assert.NotEmpty(result);

        }

    }
}

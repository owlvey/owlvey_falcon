using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SourceItemComponentTest
    {
        [Fact]
        public async Task SourceItemStartMiddle() {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var source = await ComponentTestFactory.BuildSource(container, product: product);

            var sourceComponent = container.GetInstance<SourceComponent>();
            var itemComponent = container.GetInstance<SourceItemComponent>();
            
            await itemComponent.Create(new Models.SourceItemPostRp()
            {
                SourceId = source,
                Start = OwlveyCalendar.January201905,
                End = OwlveyCalendar.January201910,
                Good = 900,
                Total = 1000
            });
            var items = await itemComponent.GetBySourceIdAndDateRange(source, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.January201908);
            Assert.NotEmpty(items);             
        }

        [Fact]
        public async Task SourceItemEndMiddle()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var source = await ComponentTestFactory.BuildSource(container, product: product);

            var sourceComponent = container.GetInstance<SourceComponent>();
            var itemComponent = container.GetInstance<SourceItemComponent>();

            await itemComponent.Create(new Models.SourceItemPostRp()
            {
                SourceId = source,
                Start = OwlveyCalendar.January201905,
                End = OwlveyCalendar.January201910,
                Good = 900,
                Total = 1000
            });
            var items = await itemComponent.GetBySourceIdAndDateRange(source, OwlveyCalendar.January201908, OwlveyCalendar.January201912);
            Assert.NotEmpty(items);
        }

        [Fact]
        public async Task SourceItemMaintenanceSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();

            await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });

            var source = await component.GetByName(product, "test");

            var itemComponent = container.GetInstance<SourceItemComponent>();

            await itemComponent.Create(new Models.SourceItemPostRp()
            {
                 SourceId = source.Id,
                 Start = DateTime.Now,
                 End = DateTime.Now,
                 Good = 900,
                 Total = 1000                
            });

            var list = await itemComponent.GetBySource(source.Id);

            Assert.NotEmpty(list);

        }
    }
}

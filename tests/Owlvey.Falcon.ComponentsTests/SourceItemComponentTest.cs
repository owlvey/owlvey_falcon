using System;
using System.Collections.Generic;
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
        public async Task SourceItemWithClues()
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
                Total = 1000,
                Clues = new Dictionary<string, decimal>() {
                    { "test", 0.3M }, { "test_a", 0.7M }
                }
            });

            await itemComponent.Create(new Models.SourceItemPostRp()
            {
                SourceId = source,
                Start = OwlveyCalendar.January201905,
                End = OwlveyCalendar.January201910,
                Good = 900,
                Total = 1000,
                Clues = new Dictionary<string, decimal>() {
                    { "test", 0.8M }, { "test_a", 0.5M }
                }
            });

            var items = await itemComponent.GetBySourceIdAndDateRange(source, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.January201908);
            Assert.NotEmpty(items);

            foreach (var item in items)
            {
                var sourceItem = await itemComponent.GetById(item.Id);
                Assert.NotEmpty(sourceItem.Clues);
            }

            var sourceRp = await sourceComponent.GetByIdWithAvailability(source, 
                OwlveyCalendar.StartJanuary2019, 
                OwlveyCalendar.January201908);

            Assert.NotEmpty(sourceRp.Clues);

            Assert.Equal(4.4M, sourceRp.Clues["test"]);
            Assert.Equal(4.8M, sourceRp.Clues["test_a"]);
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

            var item = await sourceItemComponent.Create(new Models.SourceItemPostRp()
            {
                SourceId = testSource.Id,
                Total = 1000,
                Good = 800,
                Start = OwlveyCalendar.January201904,
                End = OwlveyCalendar.January201906
            });

            var result = await sourceItemComponent.GetBySourceIdAndDateRange(testSource.Id, OwlveyCalendar.January201903, OwlveyCalendar.January201907);
            Assert.NotEmpty(result);

            result = await sourceItemComponent.GetBySourceIdAndDateRange(testSource.Id, OwlveyCalendar.January201905, OwlveyCalendar.January201907);       
            Assert.NotEmpty(result);

        }
    }
}

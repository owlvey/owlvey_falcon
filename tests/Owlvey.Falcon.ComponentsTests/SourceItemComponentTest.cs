using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SourceItemComponentTest
    {
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

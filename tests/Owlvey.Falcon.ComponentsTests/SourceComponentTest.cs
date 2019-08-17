using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SourceComponentTest
    {
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
    }
}

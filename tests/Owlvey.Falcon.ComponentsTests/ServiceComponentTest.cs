using Owlvey.Falcon.Components;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class ServiceComponentTest
    {

        [Fact]
        public async Task ServiceMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customerId, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();

            await serviceComponent.CreateService(new Models.ServicePostRp()
            {
                Name = "test",
                ProductId = productId,
                Slo = 0.95f
            }); 
            var services = await serviceQueryComponent.GetServices(productId);
            Assert.NotEmpty(services);

        }

    }
}

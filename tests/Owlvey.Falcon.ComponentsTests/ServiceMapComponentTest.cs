using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class ServiceMapComponentTest
    {
        [Fact]
        public async Task ServiceMapMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var service = await ComponentTestFactory.BuildService(container, productId);
            var feature = await ComponentTestFactory.BuildFeature(container, productId);

            var component = container.GetInstance<ServiceMapComponent>();
            
            await component.CreateServiceMap(new Models.ServiceMapPostRp() {
                 FeatureId = feature,
                 ServiceId = service
            });
            var services = await component.GetServiceMaps(service);

            Assert.NotEmpty(services);

        }
    }
}

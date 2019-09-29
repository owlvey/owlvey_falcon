using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

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
            }); 
            var services = await serviceQueryComponent.GetServices(productId);
            Assert.NotEmpty(services);

        }

        [Fact]
        public async Task IndicatorDeleteSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var dbcontext = container.GetInstance<FalconDbContext>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var indicatorComponent = container.GetInstance<IndicatorComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();
            var data = await ComponentTestFactory.BuildCustomerWithSquad(container,
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);


            await serviceComponent.DeleteService(data.serviceId);

            var service = await serviceQueryComponent.GetServiceById(data.serviceId);
            Assert.Null(service);

            var feature = await featureQueryComponent.GetFeatureById(data.featureId);
            Assert.NotNull(feature);

            var map = dbcontext.ServiceMaps.Where(c => c.ServiceId == data.serviceId).ToList();
            Assert.Empty(map);

        }

    }
}

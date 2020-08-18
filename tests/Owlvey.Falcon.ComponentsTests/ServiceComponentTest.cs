using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.ComponentsTests
{
    public class ServiceComponentTest
    {
        [Fact]
        public async Task ServiceCreateSuccess(){
            var container = ComponentTestFactory.BuildContainer();
            var (customerId, productId, serviceId, featureId, _, _) = await ComponentTestFactory.BuildCustomerWithSquad(container, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019);

            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();

            var result = await serviceComponent.CreateService(new Models.ServicePostRp(){
                 Name = "test" , ProductId = productId                
            });
            Assert.Equal(result.SLAValue.Availability, 0.99m);
            Assert.Equal(result.SLAValue.Latency, 1000m);
        }


        [Fact]
        public async Task ServiceGraphQuerySuccess() {
            var container = ComponentTestFactory.BuildContainer();
            var (customerId, productI, serviceId, featureId, _, _) = await ComponentTestFactory.BuildCustomerWithSquad(container, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019);

            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();


            var graph = await serviceQueryComponent.GetGraph(serviceId, 
                new DatePeriodValue(OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019));

            Assert.NotNull(graph);
            Assert.NotEmpty(graph.Nodes);
            Assert.NotEmpty(graph.Edges);


        }



        [Fact]
        public async Task ServiceIdempotenceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customerId, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();

            var serviceInstance = await serviceComponent.CreateService(new Models.ServicePostRp()
            {
                Name = "test",
                ProductId = productId,
            });

            serviceInstance = await serviceComponent.CreateService(new Models.ServicePostRp()
            {
                Name = "test",
                ProductId = productId,
            });
        }

        [Fact]
        public async Task ServiceMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customerId, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();

            var serviceInstance = await serviceComponent.CreateService(new Models.ServicePostRp()
            {
                Name = "test",
                ProductId = productId,                
            }); 
            var services = await serviceQueryComponent.GetServices(productId);
            Assert.NotEmpty(services);

            await serviceComponent.UpdateService(serviceInstance.Id, new Models.ServicePutRp() {
                Name = "change",
                Description = "change",
                AvailabilitySlo = 0.95m,
                LatencySlo = 2000m,
                ExperienceSlo = 0.95m,
                Avatar = "http://change.org",
                Group = "change group",
                AvailabilitySLA = 0.8m,
                LatencySLA = 800m
            });

            var serviceDetail = await serviceQueryComponent.GetServiceById(serviceInstance.Id);
            Assert.NotNull(serviceDetail);

            Assert.Equal("change", serviceDetail.Name);
            Assert.Equal("change group", serviceDetail.Group);
            Assert.Equal("change", serviceDetail.Description);
            Assert.Equal("http://change.org", serviceDetail.Avatar);
            Assert.Equal(0.95m, serviceDetail.AvailabilitySLO);            
            Assert.Equal(0.8m, serviceDetail.SLAValue.Availability);            
            Assert.Equal(800m, serviceDetail.SLAValue.Latency);            
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

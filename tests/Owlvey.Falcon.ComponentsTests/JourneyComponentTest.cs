using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.ComponentsTests
{
    public class JourneyComponentTest
    {
        [Fact]
        public async Task journeyCreateSuccess(){
            var container = ComponentTestFactory.BuildContainer();
            var (customerId, productId, journeyId, featureId, _, _) = await ComponentTestFactory.BuildCustomerWithSquad(container, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019);

            var journeyComponent = container.GetInstance<JourneyComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();

            var result = await journeyComponent.Create(new Models.JourneyPostRp(){
                 Name = "test" , ProductId = productId                
            });
            Assert.Equal(result.SLAValue.Availability, 0.99m);
            Assert.Equal(result.SLAValue.Latency, 1000m);
        }


        [Fact]
        public async Task journeyGraphQuerySuccess() {
            var container = ComponentTestFactory.BuildContainer();
            var (customerId, productI, journeyId, featureId, _, _) = await ComponentTestFactory.BuildCustomerWithSquad(container, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019);

            var journeyComponent = container.GetInstance<JourneyComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();


            var graph = await journeyQueryComponent.GetGraph(journeyId, 
                new DatePeriodValue(OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019));

            Assert.NotNull(graph);
            Assert.NotEmpty(graph.Nodes);
            Assert.NotEmpty(graph.Edges);


        }



        [Fact]
        public async Task journeyIdempotenceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customerId, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var journeyComponent = container.GetInstance<JourneyComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();

            var journeyInstance = await journeyComponent.Create(new Models.JourneyPostRp()
            {
                Name = "test",
                ProductId = productId,
            });

            journeyInstance = await journeyComponent.Create(new Models.JourneyPostRp()
            {
                Name = "test",
                ProductId = productId,
            });
        }

        [Fact]
        public async Task journeyMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customerId, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var journeyComponent = container.GetInstance<JourneyComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();

            var journeyInstance = await journeyComponent.Create(new Models.JourneyPostRp()
            {
                Name = "test",
                ProductId = productId,                
            }); 
            var journeys = await journeyQueryComponent.GetListByProductId(productId);
            Assert.NotEmpty(journeys);

            await journeyComponent.Update(journeyInstance.Id, new Models.JourneyPutRp() {
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

            var journeyDetail = await journeyQueryComponent.GetJourneyById(journeyInstance.Id);
            Assert.NotNull(journeyDetail);

            Assert.Equal("change", journeyDetail.Name);
            Assert.Equal("change group", journeyDetail.Group);
            Assert.Equal("change", journeyDetail.Description);
            Assert.Equal("http://change.org", journeyDetail.Avatar);
            Assert.Equal(0.95m, journeyDetail.AvailabilitySLO);            
            Assert.Equal(0.8m, journeyDetail.SLAValue.Availability);            
            Assert.Equal(800m, journeyDetail.SLAValue.Latency);            
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
            var journeyComponent = container.GetInstance<JourneyComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();
            var data = await ComponentTestFactory.BuildCustomerWithSquad(container,
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);


            await journeyComponent.Delete(data.journeyId);

            var journey = await journeyQueryComponent.GetJourneyById(data.journeyId);
            Assert.Null(journey);

            var feature = await featureQueryComponent.GetFeatureById(data.featureId);
            Assert.NotNull(feature);

            var map = dbcontext.JourneyMaps.Where(c => c.JourneyId == data.journeyId).ToList();
            Assert.Empty(map);

        }

    }
}

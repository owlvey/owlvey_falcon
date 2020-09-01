using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class JourneyMapComponentTest
    {
        [Fact]
        public async Task journeyMapMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var journey = await ComponentTestFactory.BuildJourney(container, productId);
            var feature = await ComponentTestFactory.BuildFeature(container, productId);

            var component = container.GetInstance<JourneyMapComponent>();
            
            await component.CreateMap(new Models.JourneyMapPostRp() {
                 FeatureId = feature,
                 JourneyId = journey
            });
            var journeys = await component.GetMaps(journey);

            Assert.NotEmpty(journeys);

        }
    }
}

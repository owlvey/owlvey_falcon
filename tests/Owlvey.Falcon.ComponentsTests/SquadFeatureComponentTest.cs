using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SquadFeatureComponentTest
    {
        [Fact]
        public async Task SquadFeatureMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var customer = await ComponentTestFactory.BuildCustomer(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();



            await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squads = await squadQueryComponent.GetSquads();

            Assert.NotEmpty(squads);
        }
    }
}


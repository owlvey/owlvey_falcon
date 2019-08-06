using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SquadComponentTest
    {
        [Fact]
        public async Task SquadMaintenanceSucces()
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

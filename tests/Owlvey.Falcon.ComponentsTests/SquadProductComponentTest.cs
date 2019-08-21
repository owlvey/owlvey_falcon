using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SquadProductComponentTest
    {
        [Fact]
        public async Task SquadProductMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var squadProductComponent = container.GetInstance<SquadProductComponent>();

            await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squad = await squadQueryComponent.GetSquadByName(customer, "test");
            
            await squadProductComponent.CreateSquadProduct(new Models.SquadProductPostRp() {
                 SquadId = squad.Id,
                 ProductId = product
            });

            var all  = await squadProductComponent.GetAll();

            Assert.NotEmpty(all);
        }
    }
}


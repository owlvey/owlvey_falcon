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

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var feature = await ComponentTestFactory.BuildFeature(container, product);
            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var squadFeatureComponent = container.GetInstance<SquadFeatureComponent>();

            await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squad = await squadQueryComponent.GetSquadByName(customer, "test");

            await squadFeatureComponent.CreateSquadFeature(new Models.SquadFeaturePostRp() {
                 SquadId = squad.Id,
                 FeatureId = feature
            });

            var all  = await squadFeatureComponent.GetAll();

            Assert.NotEmpty(all);
        }
    }
}


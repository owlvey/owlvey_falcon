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

            var squads = await squadQueryComponent.GetSquads(customer);
            Assert.NotEmpty(squads);
        }

        [Fact]
        public async Task SquadUserMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var customer = await ComponentTestFactory.BuildCustomer(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            var squadResponse = await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squad =  await squadQueryComponent.GetSquadById(squadResponse.GetResult<int>("Id"));

            var userComponent = container.GetInstance<UserComponent>();
            var userQueryComponent = container.GetInstance<UserQueryComponent>();

            var response = await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = "test@test.com"
            });

            var id = response.GetResult<int>("Id");

            var user = await userQueryComponent.GetUserById(id);

            await squadComponent.RegisterMember(squad.Id, user.Id);

            squad = await squadQueryComponent.GetSquadById(squad.Id);

           // Assert.NotEmpty(squad.Members);

            await squadComponent.UnRegisterMember(squad.Id, user.Id);

            squad = await squadQueryComponent.GetSquadById(squad.Id);

           // Assert.Empty(squad.Members);

        }
    }
}

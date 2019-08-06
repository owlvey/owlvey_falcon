using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class MemberComponentTest
    {
        [Fact]
        public async Task SquadMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customer, squad) = await ComponentTestFactory.BuildCustomerSquad(container);
            var user = await ComponentTestFactory.BuildUser(container);

            var memberComponent = container.GetInstance<MemberComponent>();

            await memberComponent.CreateMember(new Models.MemberPostRp() {
                 SquadId = squad,
                 UserId = user
            });

            var members = await memberComponent.GetMembers(squad);
            
            Assert.NotEmpty(members);
        }
    }
}

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
            var memberQueryComponent = container.GetInstance<MemberQueryComponent>();

            await memberComponent.CreateMember(new Models.MemberPostRp() {
                 SquadId = squad,
                 UserId = user
            });

            var members = await memberQueryComponent.GetMembersBySquad(squad);
            
            Assert.NotEmpty(members);
        }
    }
}

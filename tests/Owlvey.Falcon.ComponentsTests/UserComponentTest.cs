using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class UserComponentTest
    {
        [Fact]
        public async Task UserMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();
            
            var userComponent = container.GetInstance<UserComponent>();           

            await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = "test@test.com"                
            });

            var users = await userComponent.GetUsers();
            Assert.NotEmpty(users);
        }
    }
}

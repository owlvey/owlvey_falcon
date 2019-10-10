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
            var userQueryComponent = container.GetInstance<UserQueryComponent>();

            await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = "test@test.com"                
            });

            var users = await userQueryComponent.GetUsers();
            Assert.NotEmpty(users);
        }

        [Fact]
        public async Task UserMaintenanceIdempotenceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var userComponent = container.GetInstance<UserComponent>();
            var userQueryComponent = container.GetInstance<UserQueryComponent>();

            await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = "test@test.com"
            });

            var users = await userQueryComponent.GetUsers();
            Assert.NotEmpty(users);


            var response = await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = "test@test.com"
            });

            var id = response.GetResult<int>("Id");

            var result = await userQueryComponent.GetUserById(id);

            Assert.NotNull(result);
            Assert.NotNull(result.Avatar);

            await userComponent.PutUser(id, new Models.UserPutRp() {
                 Avatar = "change",
                 Email = "change"
            });

            result = await userQueryComponent.GetUserById(id);

            Assert.Equal("change", result.Avatar);


            await userComponent.DeleteUser(id);

            result = await userQueryComponent.GetUserById(id);

            Assert.Null(result);


        }
    }
}

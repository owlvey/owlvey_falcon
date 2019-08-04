using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class UserEntityTest
    {
        [Fact]
        public void CreateUserSuccess()
        {
            var entity = UserEntity.Factory.Create("test", DateTime.Now, "test@test.com");
            Assert.NotNull(entity.CreatedBy);
            Assert.NotNull(entity.CreatedOn);
            Assert.NotNull(entity.ModifiedBy);
            Assert.NotNull(entity.ModifiedOn);
            Assert.NotNull(entity.Email);
        }
    }
}

using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SquadEntityTest
    {
        [Fact]
        public void CreateUserSuccess()
        {
            var entity = SquadEntity.Factory.Create("test", DateTime.Now, "test");
            Assert.NotNull(entity.CreatedBy);
            Assert.NotNull(entity.CreatedOn);
            Assert.NotNull(entity.ModifiedBy);
            Assert.NotNull(entity.ModifiedOn);
            Assert.NotNull(entity.Name);
        }
    }
}

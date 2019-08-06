using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SquadEntityUnitTest
    {
        public SquadEntityUnitTest()
        {

        }

        [Fact]
        public void CreateSquadEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");
            var description = Guid.NewGuid().ToString("n");

            var squadEntity = SquadEntity.Factory.Create(name, DateTime.UtcNow, createdBy);

            Assert.Equal(name, squadEntity.Name);            
            Assert.Equal(createdBy, squadEntity.CreatedBy);
        }

        [Fact]
        public void CreateUserSuccess()
        {
            var entity = SquadEntity.Factory.Create("test",  DateTime.Now, "test");
            Assert.NotNull(entity.CreatedBy);
            Assert.NotNull(entity.CreatedOn);
            Assert.NotNull(entity.ModifiedBy);
            Assert.NotNull(entity.ModifiedOn);
            Assert.NotNull(entity.Name);
        }

        [Fact]
        public void CreateSquadEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;

            Assert.Throws<InvalidStateException>(() =>
            {
                SquadEntity.Factory.Create(name, DateTime.UtcNow, createdBy);
            });

        }
    }
}

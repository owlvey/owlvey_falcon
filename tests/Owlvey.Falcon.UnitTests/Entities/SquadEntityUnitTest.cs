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
            var customer = new CustomerEntity();
            var squadEntity = SquadEntity.Factory.Create(name, DateTime.UtcNow, createdBy, customer);

            Assert.Equal(name, squadEntity.Name);            
            Assert.Equal(createdBy, squadEntity.CreatedBy);
        }

        [Fact]
        public void CreateSquadSuccess()
        {
            var customer = new CustomerEntity();
            var entity = SquadEntity.Factory.Create("test",  DateTime.Now, "test", customer);
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
            var customer = new CustomerEntity();
            Assert.Throws<InvalidStateException>(() =>
            {
                SquadEntity.Factory.Create(name, DateTime.UtcNow, createdBy, customer);
            });

        }
    }
}

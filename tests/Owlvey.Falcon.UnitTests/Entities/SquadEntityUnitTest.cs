using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
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

            var squadEntity = SquadEntity.Factory.Create(name, description, createdBy);

            Assert.Equal(name, squadEntity.Name);
            Assert.Equal(description, squadEntity.Description);
            Assert.Equal(createdBy, squadEntity.CreatedBy);
        }

        [Fact]
        public void CreateSquadEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;

            Assert.Throws<ApplicationException>(() =>
            {
                SquadEntity.Factory.Create(name, description, createdBy);
            });

        }
    }
}

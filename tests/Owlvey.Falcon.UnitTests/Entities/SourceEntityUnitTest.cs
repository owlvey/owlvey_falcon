using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SourceEntityUnitTest
    {
        [Fact]
        public void CreateSourceEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var entity = SourceEntity.Factory.Create("test", 
                DateTime.UtcNow, createdBy);
            
            Assert.Equal(createdBy, entity.CreatedBy);
        }

        [Fact]
        public void CreateSourceEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var goodDefinition = string.Empty;
            var totalDefinition = string.Empty;

            Assert.Throws<InvalidStateException>(() =>
            {
                SourceEntity.Factory.Create(null, DateTime.UtcNow, createdBy);
            });

        }
    }
}

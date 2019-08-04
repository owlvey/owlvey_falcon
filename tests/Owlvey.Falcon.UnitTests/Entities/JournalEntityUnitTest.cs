using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class JournalEntityUnitTest
    {
        public JournalEntityUnitTest()
        {

        }

        [Fact]
        public void CreateJournalEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var goodDefinition = Guid.NewGuid().ToString("n");
            var totalDefinition = Guid.NewGuid().ToString("n");

            var journalEntity = JournalEntity.Factory.Create(goodDefinition, totalDefinition, DateTime.UtcNow, createdBy);

            Assert.Equal(goodDefinition, journalEntity.GoodDefinition);
            Assert.Equal(totalDefinition, journalEntity.TotalDefinition);
            Assert.Equal(createdBy, journalEntity.CreatedBy);
        }

        [Fact]
        public void CreateJournalEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var goodDefinition = string.Empty;
            var totalDefinition = string.Empty;

            Assert.Throws<ApplicationException>(() =>
            {
                JournalEntity.Factory.Create(goodDefinition, totalDefinition, DateTime.UtcNow, createdBy);
            });

        }
    }
}

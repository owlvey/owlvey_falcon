using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SourceItemEntityUnitTest
    {
        [Fact]
        public void CreateProductEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");
            var description = Guid.NewGuid().ToString("n");

            var source = TestDataFactory.BuildSource();
            var sourceItem = SourceItemEntity.Factory.Create(source, new DateTime(2019, 01, 01, 8,0,0,0), new DateTime(2019, 01, 02, 8, 0, 0, 0), 900, 1000, DateTime.Now, "test");

            Assert.NotNull(sourceItem.Source);                       
        }
    }
}

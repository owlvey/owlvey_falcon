using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SourceItemEntityCompareTest
    {

        [Fact]
        public void CompareSuccess()
        {
            var compare = new SourceItemEntityComparer();

            var result = compare.Equals(new InteractionSourceItemEntity() { Id = 2 }, new InteractionSourceItemEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new InteractionSourceItemEntity() { Id = 2 }, new InteractionSourceItemEntity() { Id = 2 });
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeSuccess()
        {
            var compare = new SourceItemEntityComparer();
            var result = compare.GetHashCode(new InteractionSourceItemEntity() { Id = 3 });
            Assert.Equal(3, result);
        }
    }
}

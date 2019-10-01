using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SourceEntityComparerTest
    {
        [Fact]
        public void CompareSuccess()
        {
            var compare = new SourceEntityComparer();

            var result = compare.Equals(new SourceEntity() { Id = 2 }, new SourceEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new SourceEntity() { Id = 2 }, new SourceEntity() { Id = 2 });
            Assert.True(result);
        }
    }
}

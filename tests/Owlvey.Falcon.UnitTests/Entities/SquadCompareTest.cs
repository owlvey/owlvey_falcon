using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SquadCompareTest
    {
        [Fact]
        public void CompareSuccess()
        {
            var compare = new SquadCompare();

            var result = compare.Equals(new SquadEntity() { Id = 2 }, new SquadEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new SquadEntity() { Id = 2 }, new SquadEntity() { Id = 2 });
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeSuccess()
        {
            var compare = new SquadCompare();
            var result = compare.GetHashCode(new SquadEntity() { Id = 3 });
            Assert.Equal(3, result);
        }
    }
}

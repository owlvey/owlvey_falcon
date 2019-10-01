using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class IndicatorCompareTest
    {
        [Fact]
        public void CompareSuccess()
        {
            var compare = new IndicatorEntityComparer();

            var result = compare.Equals(new IndicatorEntity() { Id = 2 }, new IndicatorEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new IndicatorEntity() { Id = 2 }, new IndicatorEntity() { Id = 2 });
            Assert.True(result);
        }
    }
}

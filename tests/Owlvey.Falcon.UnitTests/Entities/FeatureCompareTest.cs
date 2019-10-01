using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class FeatureCompareTest
    {
        [Fact]
        public void  CompareSuccess()
        {
            FeatureCompare compare = new FeatureCompare();

            var result = compare.Equals(new FeatureEntity() { Id = 2 }, new FeatureEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new FeatureEntity() { Id = 2 }, new FeatureEntity() { Id = 2 });
            Assert.True(result);
        }
    }
}

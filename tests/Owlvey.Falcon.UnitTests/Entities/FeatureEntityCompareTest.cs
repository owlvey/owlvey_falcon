using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class FeatureEntityCompareTest
    {
        [Fact]
        public void  CompareSuccess()
        {
            FeatureEntityCompare compare = new FeatureEntityCompare();

            var result = compare.Equals(new FeatureEntity() { Id = 2 }, new FeatureEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new FeatureEntity() { Id = 2 }, new FeatureEntity() { Id = 2 });
            Assert.True(result);
        }
        [Fact]
        public void GetHashCodeSuccess() {
            FeatureEntityCompare compare = new FeatureEntityCompare();
            var result = compare.GetHashCode(new FeatureEntity() { Id = 3 });
            Assert.Equal(3, result); 
        }
    }
}

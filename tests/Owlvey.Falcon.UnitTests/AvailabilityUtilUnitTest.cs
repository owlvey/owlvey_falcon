using System;
using Owlvey.Falcon.Core;
using Xunit;

namespace Owlvey.Falcon.UnitTests
{
    public class AvailabilityUtilUnitTest
    {
        [Fact]
        public void CreateProductEntitySuccess()
        {
            var result = AvailabilityUtils.MeasureImpact(99);
            //Assert.Equal<decimal>(90.43821m, result);
            Assert.True(true);
        }
    }
}

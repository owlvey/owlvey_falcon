using System;
using Owlvey.Falcon.Core;
using Xunit;

namespace Owlvey.Falcon.UnitTests
{
    public class AvailabilityUtilUnitTest
    {
        [Fact]
        public void MeasureImpactSuccess()
        {
            var result = AvailabilityUtils.MeasureImpact(0.90m);

            result = AvailabilityUtils.MeasureImpact(0.95m);

            result = AvailabilityUtils.MeasureImpact(0.99m);

            result = AvailabilityUtils.MeasureImpact(0.999m);
            //Assert.Equal<decimal>(90.43821m, result);
            Assert.Equal(950m, result);
        }
        [Fact]
        public void MeasureFailProportion() {
            var proportion = AvailabilityUtils.CalculateFailProportion(4, 3);

            Assert.Equal(0.25M, proportion);
        }
    }
}

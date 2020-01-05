using System;
using Owlvey.Falcon.Core;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests
{
    public class AvailabilityUtilUnitTest
    {
        [Fact]
        public void MeasureImpactSuccess()
        {
            var result = QualityUtils.MeasureImpact(0.90m);

            result = QualityUtils.MeasureImpact(0.95m);

            result = QualityUtils.MeasureImpact(0.99m);

            result = QualityUtils.MeasureImpact(0.999m);
            //Assert.Equal<decimal>(90.43821m, result);
            Assert.Equal(950m, result);
        }
        [Fact]
        public void MeasureFailProportion() {
            var proportion = QualityUtils.CalculateFailProportion(4, 3);

            Assert.Equal(0.25M, proportion);
        }

        [Fact]
        public void ProportionToMinutesSuccess() {
            var (good, total) = QualityUtils.ProportionToMinutes(OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019, 0.94M);

            var target = (decimal)good /  (decimal)(total);

            Assert.Equal(target, 0.94M);

        }
    }
}

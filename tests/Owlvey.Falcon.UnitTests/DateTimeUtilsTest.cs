using System;
using Owlvey.Falcon.Core;
using Xunit;

namespace Owlvey.Falcon.UnitTests
{
    public class DateTimeUtilsTest
    {
        [Fact]
        public void TestFormatMetric()
        {
            var result = DateTimeUtils.FormatTimeToInMinutes(61);
            Assert.Equal("00w 0d 01h 01m", result);

            result = DateTimeUtils.FormatTimeToInMinutes(75);
            Assert.Equal("00w 0d 01h 15m", result);

            result = DateTimeUtils.FormatTimeToInMinutes(120);
            Assert.Equal("00w 0d 02h 00m", result);

            result = DateTimeUtils.FormatTimeToInMinutes(11520);
            Assert.Equal("01w 1d 00h 00m", result);

            result = DateTimeUtils.FormatTimeToInMinutes(11521);
            Assert.Equal("01w 1d 00h 01m", result);

            result = DateTimeUtils.FormatTimeToInMinutes(11581);
            Assert.Equal("01w 1d 01h 01m", result);

        }
    }
}

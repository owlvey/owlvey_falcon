using System;
using Owlvey.Falcon.Core;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

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

        [Fact]
        public void CalculateBeforePreviousDatesTest() {
            var start = OwlveyCalendar.January201903;
            var end = OwlveyCalendar.January201910;

            var diff = DateTimeUtils.DaysDiff(end, start); 

            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(start, end);

            Assert.Equal(new DateTime(2018, 12, 26), ps);
            Assert.Equal(new DateTime(2019, 1, 2, 23,59,59), pe);

            Assert.Equal(diff, DateTimeUtils.DaysDiff(pe, ps));

            Assert.Equal(new DateTime(2018, 12, 18), bs);
            Assert.Equal(new DateTime(2018, 12, 25, 23, 59, 59), be);
            Assert.Equal(diff, DateTimeUtils.DaysDiff(be, bs));
        }
    }
}

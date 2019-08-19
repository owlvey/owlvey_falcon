using Owlvey.Falcon.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class DateTimeUtilsUnitTest
    {
        [Fact]
        public void MidDayDateTimeUtilSuccess()
        {
            var dt = new DateTime(2019, 2, 3, 5, 10, 58);
            var result = DateTimeUtils.DatetimeToMidDate(dt);
            Assert.Equal(12, result.Hour);
            Assert.Equal(2019, result.Year);
            Assert.Equal(2, result.Month);
            Assert.Equal(3, result.Day);
        }
    }
}

using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Values
{
    public class DatePeriodValueTest
    {
        [Fact]
        public void PeriodBuildSuccess()
        {
            var value = new DatePeriodValue(OwlveyCalendar.January201903, OwlveyCalendar.January201905);
            Assert.Equal(OwlveyCalendar.January201903.Date, value.Start);
            Assert.Equal(OwlveyCalendar.January201906.Add(-1 * TimeSpan.FromTicks(1)), value.End);
        }

        [Fact]
        public void PeriodIntervalSuccess() {
            var value = new DatePeriodValue(OwlveyCalendar.January201903, OwlveyCalendar.January201905);
            Assert.Equal(3, value.Days);            
        }

        [Fact]
        public void GetDatesSuccess()
        {
            var value = new DatePeriodValue(OwlveyCalendar.January201903, OwlveyCalendar.January201905);
            var intervals = value.GetDates();
            Assert.Equal(3, intervals.Count());
            Assert.Equal(OwlveyCalendar.January201903, intervals.ElementAt(0));
            Assert.Equal(OwlveyCalendar.January201904, intervals.ElementAt(1));
            Assert.Equal(OwlveyCalendar.January201905, intervals.ElementAt(2));
        }

        [Fact]
        public void GenerateYear() {
            var value = new DateTime(2019, 1, 15);
            var target = DatePeriodValue.ToYearFromStart(value);
            Assert.Equal(365, target.Days);
        }
        [Fact]
        public void GenerateYearPeriods() {
            var value = new DateTime(2019, 1, 15);
            var target = DatePeriodValue.ToYearFromStart(value);
            var periods = target.ToYearPeriods();
            Assert.Equal(12, periods.Count);
        }
        [Fact]
        public void GenerateDays() {
            var value = new DateTime(2019, 1, 15);
            var target = DatePeriodValue.ToYearFromStart(value);
            var periods = target.ToDaysPeriods();
            Assert.Equal(365, periods.Count);
        }
        [Fact]
        public void GenerateDaysMonth()
        {
            var target = new DatePeriodValue(new DateTime(2019, 4, 1), new DateTime(2019, 4, 30));
            var periods = target.ToDaysPeriods();
            Assert.Equal(30, periods.Count);
        }
    }
}

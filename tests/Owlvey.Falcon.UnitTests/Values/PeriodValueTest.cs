using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Values
{
    public class PeriodValueTest
    {
        [Fact]
        public void PeriodBuildSuccess() {
            PeriodValue value = new PeriodValue(OwlveyCalendar.January201903, OwlveyCalendar.January201905);
            Assert.Equal(OwlveyCalendar.January201903.Date, value.Start);
            Assert.Equal(OwlveyCalendar.January201906.Add( -1 * TimeSpan.FromSeconds(1)), value.End);
            
        }
    }
}

using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Values;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Values
{
    public class StatsValueTest
    {
        [Fact]
        public void BuildStatsValue()
        {
            var value = new StatsValue(new List<decimal>() { 1,2,3,4 });
            Assert.Equal(4, value.Max);
            Assert.Equal(1, value.Min);
            Assert.Equal(4, value.Count);
            Assert.Equal(2.5m, value.Mean);

        }
    }
}

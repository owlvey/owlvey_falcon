using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class IncidentMetricAggregateTest
    {
        [Fact]
        public void IncidentMetricAggregateSuccess() {
            var incidents = new List<IncidentEntity>();
            incidents.Add(new IncidentEntity()
            {
                TTD = 1,
                TTE = 2,
                TTF = 3
            });
            incidents.Add(new IncidentEntity()
            {
                TTD = 7,
                TTE = 8,
                TTF = 9
            });
            var agg = new IncidentMetricAggregate(incidents);
            var result = agg.Metrics();
            Assert.Equal(4, result.mttd);
            Assert.Equal(5, result.mtte);
            Assert.Equal(6, result.mttf);
            Assert.Equal(15, result.mttm);
        }
    }
}

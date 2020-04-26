using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class IndicatorAvailabilityAggregator
    {
        private IndicatorEntity Indicator;
        private DateTime Start;
        private DateTime End;        

        public IndicatorAvailabilityAggregator(IndicatorEntity indicator, 
            DateTime start, DateTime end)
        {
            this.Indicator = indicator;
            this.Start = start;
            this.End = end;            
        }

        public (IndicatorEntity, IEnumerable<DayPointValue>) MeasureAvailability() {
            var sourceAggregator = new SourceDailyAvailabilityAggregate(this.Indicator.Source, this.Start, this.End);
            var (source, items) = sourceAggregator.MeasureAvailability();
            return (this.Indicator, items);
       }
    }
}

using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class IndicatorAvailabilityAggregator
    {
        private IndicatorEntity Indicator;
        private DateTime Start;
        private DateTime End;
        private IEnumerable<SourceItemEntity> Items;

        public IndicatorAvailabilityAggregator(IndicatorEntity indicator,
            IEnumerable<SourceItemEntity> items, DateTime start, DateTime end)
        {
            this.Indicator = indicator;
            this.Start = start;
            this.End = end;
            this.Items = items;
        }

        public (IndicatorEntity indicator, decimal availability) MeasureAvailability() {
            decimal availability = 1;
            foreach (var item in this.Items)
            {
                availability *= item.Availability;                
            }            
            return (this.Indicator, availability);
        }
    }
}

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

        public (IndicatorEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability() {
            var sourceAggregator = new SourceAvailabilityAggregate(this.Indicator.Source, this.Items, this.Start, this.End);
            var (source, items) = sourceAggregator.MeasureAvailability();
            return (this.Indicator, items);
       }
    }
}

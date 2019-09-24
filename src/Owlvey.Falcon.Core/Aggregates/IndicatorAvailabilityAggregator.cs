﻿using System;
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

        public IndicatorAvailabilityAggregator(IndicatorEntity indicator, 
            DateTime start, DateTime end)
        {
            this.Indicator = indicator;
            this.Start = start;
            this.End = end;            
        }

        public (IndicatorEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability() {
            var sourceAggregator = new SourcePeriodAvailabilityAggregate(this.Indicator.Source, this.Start, this.End);
            var (source, items) = sourceAggregator.MeasureAvailability();
            return (this.Indicator, items);
       }
    }
}

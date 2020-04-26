using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class IndicatorDateAvailabilityAggregate
    {
        private readonly IndicatorEntity Indicator;                

        public IndicatorDateAvailabilityAggregate(IndicatorEntity indicator)
        {            
            this.Indicator = indicator;                        
        }        
        public ProportionMeasureValue  MeasureAvailability(DateTime? start = null, DateTime? end = null) {

            if (start.HasValue && end.HasValue)
            {
                var agg = new SourceAvailabilityAggregate(this.Indicator.Source);
                return agg.MeasureAvailability(start, end);
            }
            else {
                var agg = new SourceAvailabilityAggregate(this.Indicator.Source);
                return agg.MeasureAvailability();
            }            
        }
    }
}

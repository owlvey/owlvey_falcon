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
        public decimal MeasureAvailability() {
            var agg = new SourceDateAvailabilityAggregate(this.Indicator.Source);
            var (availability, total, good) = agg.MeasureAvailability();
            return availability;
        }
    }
}

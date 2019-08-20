using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceDateAvailabilityAggregate
    {
        private readonly SourceEntity Source;                

        public SourceDateAvailabilityAggregate(SourceEntity source)
        {            
            this.Source = source;                        
        }        
        public decimal MeasureAvailability() {
            return AvailabilityUtils.CalculateAvailability(this.Source.SourceItems.Select(c => c.Availability));            
        }
    }
}

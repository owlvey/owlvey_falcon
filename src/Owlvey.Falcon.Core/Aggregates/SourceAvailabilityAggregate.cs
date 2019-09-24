using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceAvailabilityAggregate
    {
        private readonly SourceEntity Source;                

        public SourceAvailabilityAggregate(SourceEntity source)
        {            
            this.Source = source;                        
        }        
        public (decimal availability, int total, int good) MeasureAvailability() {                        
            var total = this.Source.SourceItems.Sum(c => c.Total);
            var good = this.Source.SourceItems.Sum(c => c.Good);
            var availability = AvailabilityUtils.CalculateAvailability(total, good, 1);
            return (availability, total, good);
        }
    }
}

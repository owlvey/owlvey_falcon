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
        public (decimal availability, int total, int good) MeasureAvailability() {            

            var availability = AvailabilityUtils.CalculateAvailability(this.Source.SourceItems.Select(c => c.Availability).ToList());
            var total = this.Source.SourceItems.Sum(c => c.Total);
            var good = this.Source.SourceItems.Sum(c => c.Good);
            return (availability, total, good);
        }
    }
}

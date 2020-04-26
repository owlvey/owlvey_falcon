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
        public ProportionMeasureValue MeasureAvailability(DateTime? start = null, DateTime? end = null) {

            int total;
            int good;
            IEnumerable<SourceItemEntity> data; 
            if (start.HasValue && end.HasValue)
            {
                data = this.Source.SourceItems.Where(c => c.Target >= start && c.Target <= end);                
            }
            else
            {
                data = this.Source.SourceItems;
            }

            if (data.Count() == 0) {
                return new ProportionMeasureValue(1, false);
            }

            total = data.Sum(c => c.Total);
            good = data.Sum(c => c.Good);
            var availability = QualityUtils.CalculateAvailability(total, good, 1);
            if (this.Source.Kind == SourceKindEnum.Interaction)
            {
                return new ProportionMeasureValue(availability, total, good);
            }
            else {
                return new ProportionMeasureValue(availability);
            }           
        }
    }
}

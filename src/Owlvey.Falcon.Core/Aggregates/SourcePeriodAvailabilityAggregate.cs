using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceDailyAvailabilityAggregate
    {
        private readonly SourceEntity Source;
        private readonly DatePeriodValue Period;

        public SourceDailyAvailabilityAggregate(SourceEntity source, DatePeriodValue period)
        {            
            this.Source = source;
            this.Period = period;
        }
                

        public IEnumerable<DayMeasureValue> MeasureAvailability()
        {            
            var result = new List<DayMeasureValue>();

            foreach (var item in this.Period.GetDatesIntervals())
            {
                var agg = new SourceAvailabilityAggregate(this.Source);
                var measure = agg.MeasureAvailability(item.start, item.end);
                if (measure.HasData) {
                    result.Add(new DayMeasureValue(item.start, new QualityMeasureValue(measure.Quality) ));
                }    
            }
            return result;
        }
    }
}

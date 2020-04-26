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
        private readonly DateTime Start;
        private readonly DateTime End;        

        public SourceDailyAvailabilityAggregate(SourceEntity source, DateTime start, DateTime end)
        {            
            this.Source = source;
            this.Start = start;
            this.End = end;            
        }
                

        public (SourceEntity, IEnumerable<DayPointValue>) MeasureAvailability()
        {
            var data = this.Source.SourceItems.ToList();
            List<DayPointValue> result = new List<DayPointValue>();
            var days = DateTimeUtils.DaysDiff(End, Start);            
            var pivot = this.Start;
            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Target, pivot)).ToList();                
                if (sample.Count > 0)
                {                    
                    result.Add(new DayPointValue(pivot, sample));
                }
                pivot = pivot.AddDays(1);
            }           

            return (this.Source, result);
        }
    }
}

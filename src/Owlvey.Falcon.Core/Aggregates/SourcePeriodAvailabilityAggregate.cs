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

        internal static IEnumerable<DateAvailabilityValue> GenerateSourceItemDays(SourceItemEntity itemEntity)
        {            
            List<DateAvailabilityValue> result = new List<DateAvailabilityValue>();
            DateAvailabilityValue value = new DateAvailabilityValue();
            value.Availability = itemEntity.Availability;
            value.Date = itemEntity.Target;
            result.Add(value);
            return result;
        }                 

        public (SourceEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability()
        {
            var data = this.Source.SourceItems.SelectMany(c => GenerateSourceItemDays(c)).ToList();
            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();
            var days = DateTimeUtils.DaysDiff(End, Start);            
            var pivot = this.Start;
            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).ToList();                
                if (sample.Count > 0)
                {
                    decimal availability;
                    availability = QualityUtils.CalculateAverageAvailability(sample.Select(c => c.Availability));                    
                    result.Add(new DayAvailabilityEntity(pivot, availability, availability, availability, availability));
                }
                pivot = pivot.AddDays(1);
            }           

            return (this.Source, result);
        }
    }
}

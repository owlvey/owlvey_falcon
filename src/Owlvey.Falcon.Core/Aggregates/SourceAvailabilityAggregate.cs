using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceAvailabilityAggregate
    {
        private readonly SourceEntity Source;
        private readonly DateTime Start;
        private readonly DateTime End;
        private readonly bool FillEmpty;

        public SourceAvailabilityAggregate(SourceEntity source, DateTime start, DateTime end, bool fill=false)
        {
            this.FillEmpty = fill;
            this.Source = source;
            this.Start = start;
            this.End = end;            
        }

        internal static IEnumerable<AvailabilityValue> GenerateSourceItemDays(SourceItemEntity itemEntity)
        {
            var days = (decimal)DateTimeUtils.DaysDiff(itemEntity.End, itemEntity.Start);            
            List<AvailabilityValue> result = new List<AvailabilityValue>();

            var pivot = itemEntity.Start;
            for (int i = 0; i < days; i++)
            {
                AvailabilityValue value = new AvailabilityValue();
                value.Availability = itemEntity.Availability;
                value.Date = pivot.Date;                
                result.Add(value);
                pivot = pivot.AddDays(1);
            }

            return result;
        }           

        internal static decimal CalculatePreviousAvailability(IEnumerable<AvailabilityValue> data, DateTime start) {
            if (data.Count() == 0) { return 1; }
            var sample = data.Where(c => c.Date < start).GroupBy(c => c.Date).OrderByDescending(c => c.Key).FirstOrDefault();
            if (sample == null) { return 1; }
            return sample.Average(c => c.Availability);
        }

        public (SourceEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability()
        {
            var data = this.Source.SourceItems.SelectMany(c => GenerateSourceItemDays(c)).ToList();

            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var days = DateTimeUtils.DaysDiff(End, Start);
            var previous = CalculatePreviousAvailability(data, this.Start);
            var pivot = this.Start;
            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).ToList();
                decimal availability = previous;
                if (sample.Count > 0)
                {
                    availability = AvailabilityUtils.CalculateAvailability(sample.Select(c => c.Availability));
                    previous = availability;
                    result.Add(new DayAvailabilityEntity(pivot, availability, availability, availability, availability));
                }
                else if (this.FillEmpty)
                {
                    result.Add(new DayAvailabilityEntity(pivot, availability, availability, availability, availability));
                }

                pivot = pivot.AddDays(1);
            }           

            return (this.Source, result);
        }
    }
}

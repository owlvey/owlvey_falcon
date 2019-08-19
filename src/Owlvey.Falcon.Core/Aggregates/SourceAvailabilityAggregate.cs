using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceAvailabilityAggregate
    {
        private SourceEntity Source;
        private DateTime Start;
        private DateTime End;        

        public SourceAvailabilityAggregate(SourceEntity source, DateTime start, DateTime end)
        {
            this.Source = source;
            this.Start = start;
            this.End = end;            
        }

        internal static IEnumerable<SourceItemEntity> GenerateSourceItemDays(SourceItemEntity itemEntity)
        {
            var days = (decimal)DateTimeUtils.DaysDiff(itemEntity.End, itemEntity.Start);
            var total = (int)Math.Ceiling(itemEntity.Total / days);
            var good = (int)Math.Ceiling(itemEntity.Good / days);
            List<SourceItemEntity> result = new List<SourceItemEntity>();

            var pivot = itemEntity.Start;
            for (int i = 0; i < days; i++)
            {
                SourceItemEntity item = itemEntity.Clone();
                item.Start = pivot;
                item.Total = total;
                item.Good = good;
                item.End = DateTimeUtils.AbsoluteEnd(pivot);
                result.Add(item);
                pivot = pivot.AddDays(1);
            }

            return result;
        }

        internal static void FillEmptyAvailabilities(IEnumerable<DayAvailabilityEntity> availabilityEntities)
        {
            var data = availabilityEntities.OrderBy(c => c.Date).ToList();

            decimal default_availability;

            if (data.Any(c => c.Availability != -1))
            {
                default_availability = data.First(c => c.Availability != -1).Availability;
            }
            else
            {
                default_availability = 1;
            }

            foreach (var item in data)
            {
                if (item.Availability == -1)
                {
                    //TODO Refactoring
                    item.ChangeAvailability(default_availability);
                }
                else
                {
                    default_availability = item.Availability;
                }
            }
        }

        public (SourceEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability()
        {
            var data = this.Source.SourceItems.SelectMany(c => GenerateSourceItemDays(c)).ToList();

            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var days = DateTimeUtils.DaysDiff(End, Start);

            var pivot = this.Start;
            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Start, pivot)).ToList();

                var total = 0;
                var good = 0;

                foreach (var item in sample)
                {
                    total += item.Total;
                    good += item.Good;
                }

                var availability = AvailabilityUtils.CalculateAvailability(total, good, -1);

                result.Add(new DayAvailabilityEntity(pivot, availability, availability, availability, availability));

                pivot = pivot.AddDays(1);
            }

            FillEmptyAvailabilities(result);

            return (this.Source, result);
        }
    }
}

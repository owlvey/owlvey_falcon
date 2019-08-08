using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class IndicatorAvailabilityAggregator
    {
        private IndicatorEntity Indicator;
        private DateTime Start;
        private DateTime End;
        private IEnumerable<SourceItemEntity> Items;

        public IndicatorAvailabilityAggregator(IndicatorEntity indicator,
            IEnumerable<SourceItemEntity> items, DateTime start, DateTime end)
        {
            this.Indicator = indicator;
            this.Start = start;
            this.End = end;
            this.Items = items;
        }

        private IEnumerable<SourceItemEntity> GenerateSourceItemDays(SourceItemEntity itemEntity) {
            var days = (itemEntity.End - itemEntity.Start).TotalDays;
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

        internal static void FillEmptyAvailabilities(IEnumerable<DayAvailabilityEntity> availabilityEntities) {
            var data = availabilityEntities.OrderBy(c => c.Date).ToList();

            decimal default_availability;

            if (data.Any(c => c.Availability != -1))
            {
                default_availability = data.First(c => c.Availability != -1).Availability;
            }
            else {
                default_availability = 1;
            }
             
            foreach (var item in data)
            {
                if (item.Availability == -1)
                {
                    item.ChangeAvailability(default_availability);
                }
                else {
                    default_availability = item.Availability;
                }
            }
        }

        public (IndicatorEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability() {

            var data = this.Items.SelectMany(c => GenerateSourceItemDays(c)).ToList();

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

            return ( this.Indicator, result);
       }
    }
}

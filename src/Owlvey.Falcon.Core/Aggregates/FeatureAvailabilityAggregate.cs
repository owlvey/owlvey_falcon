using System;
using System.Collections.Generic;
using System.Linq;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class FeatureAvailabilityAggregate
    {
        private DateTime Start;
        private DateTime End; 
        public FeatureEntity Feature { get; protected set; }
        private IEnumerable<(IndicatorEntity indicator, IEnumerable<DayAvailabilityEntity> availabities)> Indicators;
        public FeatureAvailabilityAggregate(FeatureEntity entity,
            IEnumerable<(IndicatorEntity indicator, IEnumerable<DayAvailabilityEntity> Availabilities)> indicators,
            DateTime start,
            DateTime end)
        {
            this.Start = start;
            this.End = end; 
            this.Feature = entity;
            this.Indicators = indicators;
        }
        public (FeatureEntity, IEnumerable<DayAvailabilityEntity>) MeasureAvailability()
        {
            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var data = this.Indicators.SelectMany(c => c.availabities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End, this.Start);

            var pivot = this.Start; 

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).Select(c=>c.Availability).ToList();

                var availability = sample.Aggregate((a, x) => a * x);
                var minimun = sample.Min();
                var maximun = sample.Max();
                var average = sample.Average();

                result.Add(new DayAvailabilityEntity(pivot, availability, minimun, maximun, average)); 

                pivot = pivot.AddDays(1);
            }

            return (this.Feature, result);

        }
    }
}

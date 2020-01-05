using System;
using System.Collections.Generic;
using System.Linq;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class FeatureDailyAvailabilityAggregate
    {
        private DateTime Start;
        private DateTime End; 
        public FeatureEntity Feature { get; protected set; }        
        public FeatureDailyAvailabilityAggregate(FeatureEntity entity,            
            DateTime start,
            DateTime end)
        {
            this.Start = start;
            this.End = end; 
            this.Feature = entity;            
        }

        private IEnumerable<(IndicatorEntity indicator, IEnumerable<DayAvailabilityEntity> availabities)> GenerateDaily() {
            var result = new List<(IndicatorEntity, IEnumerable<DayAvailabilityEntity>)>();
            foreach (var item in this.Feature.Indicators)
            {
                var agg = new IndicatorAvailabilityAggregator(item, this.Start, this.End);
                var temp = agg.MeasureAvailability();
                result.Add(temp);
            }
            return result;
        }

        public (FeatureEntity,
            IEnumerable<DayAvailabilityEntity>,
            IEnumerable<(IndicatorEntity, IEnumerable<DayAvailabilityEntity>)>) MeasureAvailability()
        {

            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var indicators = this.GenerateDaily();

            var data = indicators.SelectMany(c => c.availabities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End, this.Start);

            var pivot = this.Start; 

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).Select(c=>c.Availability).ToList();
                decimal availability = 1;
                decimal minimun = 1;
                decimal maximun = 1;
                decimal average = 1;
                if (sample.Count != 0)
                { 
                    availability = QualityUtils.CalculateDotProportion(sample);
                    minimun = sample.Min();
                    maximun = sample.Max();
                    average = sample.Average();
                    result.Add(new DayAvailabilityEntity(pivot, availability, minimun, maximun, average));
                }
                pivot = pivot.AddDays(1);
            }

            return (this.Feature, result, indicators);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;

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

        private IEnumerable<(IndicatorEntity indicator, IEnumerable<DayPointValue> availabities)> GenerateDaily() {
            var result = new List<(IndicatorEntity, IEnumerable<DayPointValue>)>();
            foreach (var item in this.Feature.Indicators)
            {
                var agg = new IndicatorAvailabilityAggregator(item, this.Start, this.End);
                var temp = agg.MeasureAvailability();
                result.Add(temp);
            }
            return result;
        }

        public (FeatureEntity,
            IEnumerable<DayPointValue>,
            IEnumerable<(IndicatorEntity, IEnumerable<DayPointValue>)>) MeasureAvailability()
        {

            List<DayPointValue> result = new List<DayPointValue>();

            var indicators = this.GenerateDaily();

            var data = indicators.SelectMany(c => c.availabities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End, this.Start);

            var pivot = this.Start; 

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).ToList();                
                if (sample.Count != 0)
                {                     
                    result.Add(new DayPointValue(pivot, sample));
                }
                pivot = pivot.AddDays(1);
            }

            return (this.Feature, result, indicators);

        }
    }
}

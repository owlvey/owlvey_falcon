using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ServiceDailyAvailabilityAggregate
    {
        public ServiceEntity Service { get; protected set; }
        
        private readonly DateTime Start;
        private readonly DateTime End;
        public ServiceDailyAvailabilityAggregate(ServiceEntity entity,            
            DateTime Start,
            DateTime End)
        {
            this.Service = entity;            
            this.Start = Start;
            this.End = End;
        }
        private IEnumerable<(FeatureEntity feature, IEnumerable<DayPointValue> availabilities)> GenerateFeatureAvailabilities() {
            var result = new List<(FeatureEntity, IEnumerable<DayPointValue>)>();
            foreach (var item in this.Service.FeatureMap.Select(c=>c.Feature))
            {
                var agg = new FeatureDailyAvailabilityAggregate(item, this.Start, this.End);
                var (feature, availability, indicators) = agg.MeasureAvailability();
                result.Add((feature, availability));
            }
            return result;
        }
        public (ServiceEntity service,
            IEnumerable<DayPointValue> availabilities,
            IEnumerable<(FeatureEntity, IEnumerable<DayPointValue>)>) MeasureAvailability()
        {
            List<DayPointValue> result = new List<DayPointValue>();

            var indicators = this.GenerateFeatureAvailabilities();

            var data = indicators.SelectMany(c => c.availabilities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End , this.Start);

            var pivot = this.Start;

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).ToList();

                if (sample.Count > 0)
                {                    
                    result.Add(new DayPointValue(pivot, sample));
                }               

                pivot = pivot.AddDays(1);
            }

            return (this.Service, result, indicators);
            
        }
    }
}

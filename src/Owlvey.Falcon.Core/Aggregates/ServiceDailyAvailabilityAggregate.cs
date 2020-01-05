using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;

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
        private IEnumerable<(FeatureEntity feature, IEnumerable<DayAvailabilityEntity> availabilities)> GenerateFeatureAvailabilities() {
            var result = new List<(FeatureEntity, IEnumerable<DayAvailabilityEntity>)>();
            foreach (var item in this.Service.FeatureMap.Select(c=>c.Feature))
            {
                var agg = new FeatureDailyAvailabilityAggregate(item, this.Start, this.End);
                var (feature, availability, indicators) = agg.MeasureAvailability();
                result.Add((feature, availability));
            }
            return result;
        }
        public (ServiceEntity service,
            IEnumerable<DayAvailabilityEntity> availabilities,
            IEnumerable<(FeatureEntity, IEnumerable<DayAvailabilityEntity>)>) MeasureAvailability()
        {
            List<DayAvailabilityEntity> result = new List<DayAvailabilityEntity>();

            var indicators = this.GenerateFeatureAvailabilities();

            var data = indicators.SelectMany(c => c.availabilities).ToList();

            var days = DateTimeUtils.DaysDiff(this.End , this.Start);

            var pivot = this.Start;

            for (int i = 0; i < days; i++)
            {
                var sample = data.Where(c => DateTimeUtils.CompareDates(c.Date, pivot)).Select(c => c.Availability).ToList();

                decimal availability = 1;
                decimal minimun = 1;
                decimal maximun = 1;
                decimal average = 1;

                if (sample.Count > 0)
                {
                    if (this.Service.Aggregation == ServiceAggregationEnum.Minimun)
                    {
                        availability = QualityUtils.CalculateMinimumAvailability(sample);
                    }
                    else {
                        availability = QualityUtils.CalculateDotProportion(sample);
                    }
                    
                    minimun = sample.Min();
                    maximun = sample.Max();
                    average = sample.Average();
                }

                

                result.Add(new DayAvailabilityEntity(pivot, availability, minimun, maximun, average));

                pivot = pivot.AddDays(1);
            }

            return (this.Service, result, indicators);
            
        }
    }
}

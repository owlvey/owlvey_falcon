using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ServiceDailyAggregate
    {
        public ServiceEntity Service { get; protected set; }
                
        private readonly DatePeriodValue Period;
        public ServiceDailyAggregate(ServiceEntity entity, DatePeriodValue period)
        {
            this.Service = entity;
            this.Period = period;
        }
        
        public (
            IEnumerable<DayMeasureValue> serviceDaily,
            IEnumerable<(FeatureEntity, IEnumerable<DayMeasureValue>)> featuresDaily) MeasureQuality()
        {

            List<DayMeasureValue> serviceResult = new List<DayMeasureValue>();

            foreach (var item in this.Period.GetDatesIntervals())
            {
                var agg = new ServiceQualityAggregate(this.Service);
                var measure = agg.MeasureQuality(item.start, item.end);
                if (measure.HasData) {
                    serviceResult.Add(new DayMeasureValue(item.start, measure));
                }                
            }

            var featuresResult = new List<(FeatureEntity, IEnumerable<DayMeasureValue>)>();

            foreach (var map in this.Service.FeatureMap)
            {
                List<DayMeasureValue> temp = new List<DayMeasureValue>();
                foreach (var item in this.Period.GetDatesIntervals())
                {
                    var agg = new FeatureAvailabilityAggregate(map.Feature);
                    var measure = agg.MeasureQuality(item.start, item.end);
                    if (measure.HasData) {
                        temp.Add(new DayMeasureValue(item.start, measure));
                    }                    
                }
                featuresResult.Add( (map.Feature, temp) );
            }
            return (serviceResult, featuresResult);            
        }
    }
}

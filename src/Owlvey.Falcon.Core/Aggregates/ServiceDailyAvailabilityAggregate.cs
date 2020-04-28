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

            foreach (var period in this.Period.GetDatesPeriods())
            {                
                var measure = this.Service.MeasureQuality(period);
                if (measure.HasData) {
                    serviceResult.Add(new DayMeasureValue(period.Start, measure));
                }                
            }

            var featuresResult = new List<(FeatureEntity, IEnumerable<DayMeasureValue>)>();

            foreach (var map in this.Service.FeatureMap)
            {
                List<DayMeasureValue> temp = new List<DayMeasureValue>();
                foreach (var item in this.Period.GetDatesPeriods())
                {                    
                    var measure = map.Feature.MeasureQuality(item);
                    if (measure.HasData) {
                        temp.Add(new DayMeasureValue(item.Start, measure));
                    }                    
                }
                featuresResult.Add( (map.Feature, temp) );
            }
            return (serviceResult, featuresResult);            
        }
    }
}

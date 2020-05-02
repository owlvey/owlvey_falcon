using System;
using System.Collections.Generic;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class FeatureDailyAvailabilityAggregate
    {
        
        private DatePeriodValue Period; 
        public FeatureEntity Feature { get; protected set; }        
        public FeatureDailyAvailabilityAggregate(FeatureEntity entity,
            DatePeriodValue period)
        {
            this.Period = period;
            this.Feature = entity;            
        }


        public (IEnumerable<DayMeasureValue>,
            IEnumerable<(IndicatorEntity, IEnumerable<DayMeasureValue>)>) MeasureAvailability()
        {

            var featureResult = new List<DayMeasureValue>();

            foreach (var item in this.Period.GetDatesPeriods())
            {                
                var measure = this.Feature.MeasureQuality(item);
                if (measure.HasData) 
                    featureResult.Add(new DayMeasureValue(item.Start, measure)); 
            }

            var indicatorResult = new List<(IndicatorEntity, IEnumerable<DayMeasureValue>)>();
            foreach (var indicator in this.Feature.Indicators)
            {
                var temporal = new List<DayMeasureValue>();
                foreach (var period in this.Period.GetDatesPeriods())
                {                    
                    var measure = indicator.Source.MeasureProportion(period);
                    if (measure.HasData) 
                        temporal.Add(new DayMeasureValue(period.Start, 
                            new QualityMeasureValue(measure.Proportion)));
                }
                indicatorResult.Add( (indicator, temporal ));
            }

            return (featureResult, indicatorResult);

        }
    }
}

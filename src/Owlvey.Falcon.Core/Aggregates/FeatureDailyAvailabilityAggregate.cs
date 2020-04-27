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

            foreach (var item in this.Period.GetDatesIntervals())
            {
                var agg = new Core.Aggregates.FeatureAvailabilityAggregate(this.Feature);
                var measure = agg.MeasureQuality(item.start, item.end);
                if (measure.HasData) featureResult.Add(new DayMeasureValue(item.start, measure)); 
            }

            var indicatorResult = new List<(IndicatorEntity, IEnumerable<DayMeasureValue>)>();
            foreach (var indicator in this.Feature.Indicators)
            {
                var temporal = new List<DayMeasureValue>();
                foreach (var (start, end) in this.Period.GetDatesIntervals())
                {
                    var agg = new SourceAvailabilityAggregate(indicator.Source);
                    var measure = agg.MeasureAvailability(start, end);
                    if (measure.HasData) 
                        temporal.Add(new DayMeasureValue(start, new QualityMeasureValue(measure.Quality)));
                }
                indicatorResult.Add( (indicator, temporal ));
            }

            return (featureResult, indicatorResult);

        }
    }
}

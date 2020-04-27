using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class FeatureAvailabilityAggregate
    {
        private readonly FeatureEntity Feature;                

        public FeatureAvailabilityAggregate(FeatureEntity feature)
        {            
            this.Feature = feature;                        
        }
       
        public QualityMeasureValue  MeasureQuality(DateTime? start = null, DateTime? end = null) {
            
            var result = new List<decimal>();
            var availability = new List<decimal>();
            var latency = new List<decimal>();
            foreach (var item in this.Feature.Indicators)
            {
                var agg = new IndicatorQualityAggregate(item);

                QualityMeasureValue proportion;                
                if (start.HasValue && end.HasValue) {
                    proportion = agg.MeasureAvailability(start, end);
                }
                else{
                    proportion = agg.MeasureAvailability();
                }

                if (proportion.HasData) {
                    result.Add(proportion.Quality);
                    if (item.Source.Group == SourceGroupEnum.Availability)
                    {
                        availability.Add(proportion.Quality);
                    }
                    else if (item.Source.Group == SourceGroupEnum.Latency)
                    {
                        latency.Add(proportion.Quality);
                    }
                }                
            }
            if (result.Count > 0)
            {                
                return new QualityMeasureValue(
                    QualityUtils.CalculateMinimumAvailability(result, round: 3),
                    QualityUtils.CalculateMinimumAvailability(availability, 3),
                    QualityUtils.CalculateMinimumAvailability(latency, 3), true);
            }
            else {
                return new QualityMeasureValue(1, 1, 1, false);
            }            
        }
    }
}

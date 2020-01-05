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
        public (decimal quality, int total, int good, decimal tavailability, decimal tlatency) MeasureQuality() {
            var result = new List<decimal>();
            var availability = new List<decimal>();
            var latency = new List<decimal>();
            int sumTotal = 0;
            int sumGood = 0; 
            foreach (var item in this.Feature.Indicators)
            {
                var agg = new IndicatorDateAvailabilityAggregate(item);
                var (proportion, total, good) = agg.MeasureAvailability();
                result.Add(proportion);
                if (item.Source.Group == SourceGroupEnum.Availability)
                {
                    availability.Add(proportion);
                }
                else if (item.Source.Group == SourceGroupEnum.Latency) {
                    latency.Add(proportion);
                }
                sumTotal += total;
                sumGood += good;
            }
            if (result.Count > 0)
            {
                return (QualityUtils.CalculateDotProportion(result, round: 4), sumTotal, 
                    sumGood, QualityUtils.CalculateDotProportion(availability, 4),
                             QualityUtils.CalculateDotProportion(latency, 4)
                    );
            }
            else {
                return (1, 0, 0, 1, 1);
            }            
        }
    }
}

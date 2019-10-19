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
        public (decimal availability, int total, int good) MeasureAvailability() {
            var result = new List<decimal>();
            int sumTotal = 0;
            int sumGood = 0; 
            foreach (var item in this.Feature.Indicators)
            {
                var agg = new IndicatorDateAvailabilityAggregate(item);
                var (availability, total, good) = agg.MeasureAvailability();
                result.Add(availability);
                sumTotal += total;
                sumGood += good;
            }
            if (result.Count > 0)
            {
                return (AvailabilityUtils.CalculateDotAvailability(result), sumTotal, sumGood);
            }
            else {
                return (1, 0, 0);
            }            
        }
    }
}

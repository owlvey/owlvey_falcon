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
        public decimal MeasureAvailability() {
            var result = new List<Decimal>();
            foreach (var item in this.Feature.Indicators)
            {
                var agg = new IndicatorDateAvailabilityAggregate(item);
                result.Add(agg.MeasureAvailability());
            }
            if (result.Count > 0)
            {
                return AvailabilityUtils.CalculateDotAvailability(result);
            }
            else {
                return 1;
            }            
        }
    }
}

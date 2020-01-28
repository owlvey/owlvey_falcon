using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ServiceAvailabilityAggregate
    {
        private readonly ServiceEntity Service;                

        public ServiceAvailabilityAggregate(ServiceEntity service)
        {            
            this.Service = service;                        
        }        
        public (decimal availability, int total, int good) MeasureAvailability() {
            var result = new List<decimal>();
            int sumTotal = 0;
            int sumGood = 0;

            foreach (var map in this.Service.FeatureMap)
            {
                var agg = new FeatureAvailabilityAggregate(map.Feature);
                var (availability, total, good, _, _) = agg.MeasureQuality();
                result.Add(availability);
                sumTotal += total;
                sumGood += good; 
            }
            
            if (result.Count > 0)
            {
                if (this.Service.Aggregation == ServiceAggregationEnum.Minimun)
                {
                    return (QualityUtils.CalculateMinimumAvailability(result, round: 3), sumTotal, sumGood);
                }
                else if (this.Service.Aggregation == ServiceAggregationEnum.Average) {
                    return (QualityUtils.CalculateAverageAvailability(result, round: 3), sumTotal, sumGood);
                }
                else
                {
                    return (QualityUtils.CalculateDotProportion(result, round: 3), sumTotal, sumGood);
                }
                
            }
            else {
                return (1, 0,0);
            }            
        }
    }
}

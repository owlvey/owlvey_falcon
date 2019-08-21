using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ServiceDateAvailabilityAggregate
    {
        private readonly ServiceEntity Service;                

        public ServiceDateAvailabilityAggregate(ServiceEntity service)
        {            
            this.Service = service;                        
        }        
        public decimal MeasureAvailability() {
            var result = new List<Decimal>();
            foreach (var map in this.Service.FeatureMap)
            {
                var agg = new FeatureDateAvailabilityAggregate(map.Feature);
                result.Add(agg.MeasureAvailability());                
            }
            
            if (result.Count > 0)
            {
                return AvailabilityUtils.CalculateAvailability(result);
            }
            else {
                return 1;
            }            
        }
    }
}

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
        public decimal MeasureAvailability() {
            var result = new List<Decimal>();
            foreach (var map in this.Service.FeatureMap)
            {
                var agg = new FeatureAvailabilityAggregate(map.Feature);
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

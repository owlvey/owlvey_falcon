using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ServiceAvailabilityAggregate
    {
        public ServiceEntity Service { get; protected set; }
        private IEnumerable<(FeatureEntity indicator, decimal Availability)> Features;
        public ServiceAvailabilityAggregate(ServiceEntity entity, IEnumerable<(FeatureEntity indicator, decimal Availability)> indicators)
        {
            this.Service = entity;
            this.Features = indicators;
        }
        public (ServiceEntity feature, decimal availability) MeasureAvailability()
        {
            decimal availability = 1;
            foreach (var (_, item) in this.Features)
            {
                availability *= item;
            }
            return (this.Service, availability);
        }
    }
}

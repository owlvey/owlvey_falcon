using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class FeatureAvailabilityAggregate
    {        
        public FeatureEntity Feature { get; protected set; }
        private IEnumerable<(IndicatorEntity indicator, decimal Availability)> Indicators;
        public FeatureAvailabilityAggregate(FeatureEntity entity, IEnumerable<(IndicatorEntity indicator, decimal Availability)> indicators)
        {
            this.Feature = entity;
            this.Indicators = indicators;
        }
        public (FeatureEntity feature, decimal availability) MeasureAvailability()
        {
            decimal availability = 1;
            foreach (var (indicator, item) in this.Indicators)
            {
                availability *= item;
            }
            return (this.Feature, availability);
        }
    }
}

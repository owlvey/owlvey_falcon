using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class ServiceAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {
            var (_, product, service, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
           
            var features = new List<(FeatureEntity feature, decimal availability)>
            {
                (feature, 0.9m),
                (feature, 0.9m)
            };
            var aggregate = new ServiceAvailabilityAggregate(service, features);
            var (_, availability) = aggregate.MeasureAvailability();

            Assert.Equal(availability, (decimal)(0.9 * 0.9));
        }
    }
}

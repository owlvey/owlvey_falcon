using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class ProductAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureProductAvailability()
        {
            var (_, product, service, _) = TestDataFactory.BuildCustomerProductServiceFeature();

            var features = new List<(ServiceEntity service, decimal availability)>
            {
                (service, 0.9m),
                (service, 0.9m)
            };

            var aggregate = new ProductAvailabilityAggregate(product, features);
            var (_, availability) = aggregate.MeasureAvailability();
            Assert.Equal(availability, (decimal)(0.9 * 0.9));
        }
    }
}

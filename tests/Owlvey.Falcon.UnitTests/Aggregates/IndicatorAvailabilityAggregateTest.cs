using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class IndicatorAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {

            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source, DateTime.Now, DateTime.Now, 900, 1000, DateTime.Now, "test");            
            var sourceItemB = SourceItemEntity.Factory.Create(source, DateTime.Now, DateTime.Now, 900, 1000, DateTime.Now, "test");
            
            var aggregate = new IndicatorAvailabilityAggregator(indicator, new [] { sourceItemA, sourceItemB }, DateTime.Now, DateTime.Now);

            var (_, availability) = aggregate.MeasureAvailability();

            Assert.Equal((decimal)(0.9 * 0.9), availability);
        }
    }
}

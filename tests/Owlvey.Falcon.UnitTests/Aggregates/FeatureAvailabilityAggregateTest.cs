using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class FeatureAvailabilityAggregateTest
    {

        [Fact]
        public void MeasureFeatureAvailability() {

            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItem = SourceItemEntity.Factory.Create(source, DateTime.Now, DateTime.Now, 900, 1000, DateTime.Now, "test");
            indicator.Source.SourceItems.Add(sourceItem);
            sourceItem = SourceItemEntity.Factory.Create(source, DateTime.Now, DateTime.Now, 900, 1000, DateTime.Now, "test");
            indicator.Source.SourceItems.Add(sourceItem);
            feature.Indicators.Add(indicator);

            var indicators = new List<(IndicatorEntity indicator, decimal availability)>
            {
                (indicator, 0.9m),
                (indicator, 0.9m)
            };

            var aggregate = new FeatureAvailabilityAggregate(feature, indicators);

            var (_, availability) = aggregate.MeasureAvailability();
            Assert.Equal(availability, (decimal)(0.9 * 0.9));

        }
    }
}

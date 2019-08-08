using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using TDF = Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class CustomerAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItem = SourceItemEntity.Factory.Create(source, TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            var indicatorA = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItemA = SourceItemEntity.Factory.Create(source, TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            var indicator_availability = new IndicatorAvailabilityAggregator(indicator,
                new List<SourceItemEntity>() { sourceItem },
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019);

            var indicator_availabilities = indicator_availability.MeasureAvailability();

            var indicator_availabilityA = new IndicatorAvailabilityAggregator(indicatorA,
                new List<SourceItemEntity>() { sourceItemA },
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019);

            var indicatorA_availabilities = indicator_availabilityA.MeasureAvailability();

            var aggregate = new FeatureAvailabilityAggregate(feature,
                new[] { indicator_availabilities, indicatorA_availabilities },
                TDF.Calendar.StartJanuary2019, TDF.Calendar.EndJanuary2019);

            var (_, features_availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, features_availabilities.Count());
        }
    }
}

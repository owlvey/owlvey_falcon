using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using TDF  = Owlvey.Falcon.UnitTests.TestDataFactory;
using Owlvey.Falcon.Core;
using System.Collections.Generic;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class IndicatorAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureAvailability()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");            
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.StartJanuary2019, TDF.Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            
            var aggregate = new IndicatorAvailabilityAggregator(indicator,
                new [] { sourceItemA, sourceItemB },
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.75m, availabilities.First().Availability);
        }

        [Fact]
        public void MeasureAvailabilityNoDataFirstDates()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.January201905,
                TDF.Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.January201905, TDF.Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            var aggregate = new IndicatorAvailabilityAggregator(indicator,
                new[] { sourceItemA, sourceItemB },
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.745m, availabilities.First().Availability);
        }

        [Fact]
        public void MeasureAvailabilityEmpty() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");
            
            var aggregate = new IndicatorAvailabilityAggregator(
                indicator,
                new List<SourceItemEntity>(),
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();
            Assert.Equal(31, availabilities.Count());
            Assert.Equal(1m, availabilities.First().Availability);
        }

        [Fact]
        public void MeasureFeatureAvailabilityEmptyPeriods()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.StartJanuary2019, TDF.Calendar.January201903,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.January201905, TDF.Calendar.January201910,
                800, 1000, DateTime.Now, "test");
            var sourceItemC = SourceItemEntity.Factory.Create(source,
                TDF.Calendar.January201920, TDF.Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            var aggregate = new IndicatorAvailabilityAggregator(indicator,
                new[] { sourceItemA, sourceItemB, sourceItemC },
                TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.75m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201903)).Availability);
            Assert.Equal(0.75m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201904)).Availability);

            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201905)).Availability);
            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201906)).Availability);
            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201907)).Availability);

            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201912)).Availability);
            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.Calendar.January201914)).Availability);
        }
    }
}

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
                TDF.OwlveyCalendar.StartJanuary2019,
                TDF.OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");            
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.OwlveyCalendar.StartJanuary2019, TDF.OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);

            var aggregate = new IndicatorAvailabilityAggregator(indicator,                
                TDF.OwlveyCalendar.StartJanuary2019,
                TDF.OwlveyCalendar.EndJanuary2019);

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
                TDF.OwlveyCalendar.January201905,
                TDF.OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.OwlveyCalendar.January201905,
                TDF.OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);

            var aggregate = new IndicatorAvailabilityAggregator(indicator,                
                TDF.OwlveyCalendar.StartJanuary2019,
                TDF.OwlveyCalendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(1m, availabilities.ElementAt(0).Availability);
            Assert.Equal(0.75m, availabilities.ElementAt(4).Availability);            
        }

        [Fact]
        public void MeasureAvailabilityEmpty() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");
            
            var aggregate = new IndicatorAvailabilityAggregator(
                indicator,                
                TDF.OwlveyCalendar.StartJanuary2019,
                TDF.OwlveyCalendar.EndJanuary2019);

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
                TDF.OwlveyCalendar.StartJanuary2019, TDF.OwlveyCalendar.January201903, 900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.OwlveyCalendar.January201905, TDF.OwlveyCalendar.January201910, 800, 1000, DateTime.Now, "test");
            var sourceItemC = SourceItemEntity.Factory.Create(source,
                TDF.OwlveyCalendar.January201920, TDF.OwlveyCalendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);
            source.SourceItems.Add(sourceItemC);

            var aggregate = new IndicatorAvailabilityAggregator(indicator,                
                TDF.OwlveyCalendar.StartJanuary2019,
                TDF.OwlveyCalendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.75m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201903)).Availability);            
            Assert.Equal(0.75m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201904)).Availability);

            Assert.Equal(0.800m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201905)).Availability);
            Assert.Equal(0.800m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201906)).Availability);
            Assert.Equal(0.800m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201907)).Availability);

            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201912)).Availability);
            Assert.Equal(0.8m, availabilities.Single(c => DateTimeUtils.CompareDates(c.Date, TDF.OwlveyCalendar.January201914)).Availability);            
        }
    }
}

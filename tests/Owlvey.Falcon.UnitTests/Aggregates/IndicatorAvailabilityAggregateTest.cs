using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using TDF  = Owlvey.Falcon.UnitTests.TestDataFactory;
using Owlvey.Falcon.Core;
using System.Collections.Generic;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

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
                OwlveyCalendar.January201905,
                OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201905,
                OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);

            var aggregate = new IndicatorAvailabilityAggregator(indicator,                
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(27, availabilities.Count());
            Assert.Equal(0.75m, availabilities.ElementAt(0).Availability);
            Assert.Equal(0.75m, availabilities.ElementAt(4).Availability);            
        }

        [Fact]
        public void MeasureAvailabilityEmpty() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");
            
            var aggregate = new IndicatorAvailabilityAggregator(
                indicator,                
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();
            Assert.Empty(availabilities);            
        }

        [Fact]
        public void MeasureFeatureAvailabilityEmptyPeriods()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.January201903, 900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201905, OwlveyCalendar.January201910, 800, 1000, DateTime.Now, "test");
            var sourceItemC = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201920, OwlveyCalendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);
            source.SourceItems.Add(sourceItemC);

            var aggregate = new IndicatorAvailabilityAggregator(indicator,                
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(21, availabilities.Count());
            Assert.Equal(0.75m, availabilities.Single(
                c => DateTimeUtils.CompareDates(c.Date, OwlveyCalendar.January201903)).Availability);                        
            Assert.Equal(0.800m, availabilities.Single(
                c => DateTimeUtils.CompareDates(c.Date, OwlveyCalendar.January201905)).Availability);
            Assert.Equal(0.800m, availabilities.Single(
                c => DateTimeUtils.CompareDates(c.Date, OwlveyCalendar.January201906)).Availability);
            Assert.Equal(0.800m, availabilities.Single(
                c => DateTimeUtils.CompareDates(c.Date, OwlveyCalendar.January201907)).Availability);
            Assert.Equal(0.75m, availabilities.Single(
                c => DateTimeUtils.CompareDates(c.Date, OwlveyCalendar.January201920)).Availability);            
        }
    }
}

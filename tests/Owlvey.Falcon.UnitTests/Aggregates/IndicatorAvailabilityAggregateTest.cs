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

            var sourceItemA = SourceEntity.Factory.CreateItemsFromRange(source,
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);            
            var sourceItemB = SourceEntity.Factory.CreateItem(source,
                TDF.OwlveyCalendar.StartJanuary2019,
                900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);

            foreach (var item in sourceItemA)
            {
                source.SourceItems.Add(item);
            }
            
            source.SourceItems.Add(sourceItemB);

            var aggregate = new SourceDailyAvailabilityAggregate(indicator.Source,                
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019));

            var availabilities = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.76m, availabilities.First().Measure.Availability);
        }

        [Fact]
        public void MeasureAvailabilityNoDataFirstDates()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceEntity.Factory.CreateItem(source,
                OwlveyCalendar.January201905, 900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);
            var sourceItemB = SourceEntity.Factory.CreateItem(source,                
                OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);

            var aggregate = new SourceDailyAvailabilityAggregate(indicator.Source,                
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019));

            var availabilities = aggregate.MeasureAvailability();

            Assert.Equal(2, availabilities.Count());
            Assert.Equal(0.75m, availabilities.ElementAt(0).Measure.Availability);
            Assert.Equal(0.75m, availabilities.ElementAt(1).Measure.Availability);            
        }

        [Fact]
        public void MeasureAvailabilityEmpty() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");
            
            var aggregate = new SourceDailyAvailabilityAggregate(
                indicator.Source,                
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019));

            var availabilities = aggregate.MeasureAvailability();
            Assert.Empty(availabilities);            
        }
       
    }
}

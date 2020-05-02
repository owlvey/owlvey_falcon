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

            var sourceItemA = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");            
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                TDF.OwlveyCalendar.StartJanuary2019,
                900, 1200, DateTime.Now, "test");

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
            Assert.Equal(0.751m, availabilities.First().Measure.Quality);
        }

        [Fact]
        public void MeasureAvailabilityNoDataFirstDates()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201905,                
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,                
                OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);

            var aggregate = new SourceDailyAvailabilityAggregate(indicator.Source,                
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019));

            var availabilities = aggregate.MeasureAvailability();

            Assert.Equal(2, availabilities.Count());
            Assert.Equal(0.75m, availabilities.ElementAt(0).Measure.Quality);
            Assert.Equal(0.75m, availabilities.ElementAt(1).Measure.Quality);            
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

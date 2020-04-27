using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class SourcePeriodAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureAvailabilityWithNoDataStart()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var sourceItemA = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.January201903,
                OwlveyCalendar.January201905,
                900, 1000, DateTime.Now, "test");

            var sourceItemB = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.January201908,
                OwlveyCalendar.January201910,
                800, 1000, DateTime.Now, "test");


            foreach (var item in sourceItemA)
            {
                source.SourceItems.Add(item);
            }

            foreach (var item in sourceItemB)
            {
                source.SourceItems.Add(item);
            }                                   

            var aggregate = new SourceDailyAvailabilityAggregate(source,
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.January201906,
                OwlveyCalendar.January201920));

            var result = aggregate.MeasureAvailability();

            Assert.Equal(3, result.Count());
            Assert.Equal(0.79940M, result.ElementAt(0).Measure.Quality);
            Assert.Equal(0.79940M, result.ElementAt(1).Measure.Quality);
            Assert.Equal(0.79940M, result.ElementAt(2).Measure.Quality);            
        }

        [Fact]
        public void MeasureAvailabilityWithPartialStart() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var sourceItemA = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.January201908,
                OwlveyCalendar.January201914,
                900, 1000, DateTime.Now, "test");

            foreach (var item in sourceItemA)
            {
                source.SourceItems.Add(item);
            }

            
            var aggregate = new SourceDailyAvailabilityAggregate(source,
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.January201905,
                OwlveyCalendar.January201910));

            var result = aggregate.MeasureAvailability();

            Assert.Equal(3, result.Count());            
            Assert.Equal(0.90210M, result.ElementAt(0).Measure.Quality);
            Assert.Equal(0.90210M, result.ElementAt(1).Measure.Quality);
            Assert.Equal(0.90210M, result.ElementAt(2).Measure.Quality);
        }

        [Fact]
        public void MeasureAvailabilityWithPartialEnd()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var sourceItemA = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.January201908,
                OwlveyCalendar.January201914,
                900, 1000, DateTime.Now, "test");

            foreach (var item in sourceItemA)
            {
                source.SourceItems.Add(item);
            }            

            var aggregate = new SourceDailyAvailabilityAggregate(source,
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.January201910,
                OwlveyCalendar.January201920));

            var result = aggregate.MeasureAvailability();

            Assert.Equal(5, result.Count());
            Assert.Equal(0.90210M, result.First().Measure.Quality);
        }
             

        [Fact]
        public void MeasureAvailability()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            foreach (var item in sourceItemA)
            {
                source.SourceItems.Add(item);
            }

            foreach (var item in sourceItemB)
            {
                source.SourceItems.Add(item);
            }            

            var aggregate = new SourceDailyAvailabilityAggregate(source,                
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019));

            var availabilities = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.76923m, availabilities.First().Measure.Quality);
        }

        [Fact]
        public void MeasureAvailabilityNotMinusOne()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.CreateFromRange(source,
                OwlveyCalendar.StartJuly2019,
                OwlveyCalendar.EndJuly2019,
                3900223, 3911869, DateTime.Now, "test");

            foreach (var item in sourceItemA)
            {
                source.SourceItems.Add(item);
            }            

            var aggregate = new SourceDailyAvailabilityAggregate(source,
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJuly2019,
                OwlveyCalendar.EndJuly2019));

            var availabilities = aggregate.MeasureAvailability();
            
            Assert.Equal(31, availabilities.Count());
            foreach (var item in availabilities)
            {
                Assert.NotEqual(-1, item.Measure.Quality);                          
            }            
        }
    }
}

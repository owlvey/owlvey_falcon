using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class SourceAvailabilityAggregateTest
    {


        [Fact]
        public void MeasureAvailabilityWithNoDataStart()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201903,
                OwlveyCalendar.January201905,
                900, 1000, DateTime.Now, "test");

            var sourceItemB = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201908,
                OwlveyCalendar.January201910,
                800, 1000, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB);

            var aggregate = new SourceAvailabilityAggregate(source,
                OwlveyCalendar.January201906,
                OwlveyCalendar.January201920);

            var result = aggregate.MeasureAvailability();

            Assert.Equal(3, result.Item2.Count());
            Assert.Equal(0.8M, result.Item2.ElementAt(0).Availability);
            Assert.Equal(0.8M, result.Item2.ElementAt(1).Availability);
            Assert.Equal(0.8M, result.Item2.ElementAt(2).Availability);            
        }

        [Fact]
        public void MeasureAvailabilityWithPartialStart() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201908,
                OwlveyCalendar.January201914,
                900, 1000, DateTime.Now, "test");            

            source.SourceItems.Add(sourceItemA);

            var aggregate = new SourceAvailabilityAggregate(source,
                OwlveyCalendar.January201905,
                OwlveyCalendar.January201910);

            var result = aggregate.MeasureAvailability();

            Assert.Equal(3, result.Item2.Count());            
            Assert.Equal(0.9M, result.Item2.ElementAt(0).Availability);
            Assert.Equal(0.9M, result.Item2.ElementAt(1).Availability);
            Assert.Equal(0.9M, result.Item2.ElementAt(2).Availability);
        }

        [Fact]
        public void MeasureAvailabilityWithPartialEnd()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201908,
                OwlveyCalendar.January201914,
                900, 1000, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);

            var aggregate = new SourceAvailabilityAggregate(source,
                OwlveyCalendar.January201910,
                OwlveyCalendar.January201920);

            var result = aggregate.MeasureAvailability();

            Assert.Equal(5, result.Item2.Count());
            Assert.Equal(0.9M, result.Item2.First().Availability);
        }

        [Fact]
        public void GenerateSourceItemDaysTest() {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);
            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.January201905,
                OwlveyCalendar.January201910,
                900, 1000, DateTime.Now, "test");

            var result = SourceAvailabilityAggregate.GenerateSourceItemDays(sourceItemA);
            Assert.Equal(6, result.Count());
            foreach (var item in result)
            {
                Assert.Equal(900M/1000M, item.Availability);                
            }
        }


        [Fact]
        public void MeasureAvailability()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB );

            var aggregate = new SourceAvailabilityAggregate(source,                
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.75m, availabilities.First().Availability);
        }

        [Fact]
        public void MeasureAvailabilityNotMinusOne()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                OwlveyCalendar.StartJuly2019,
                OwlveyCalendar.EndJuly2019,
                3900223, 3911869, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);

            var aggregate = new SourceAvailabilityAggregate(source,
                OwlveyCalendar.StartJuly2019,
                OwlveyCalendar.EndJuly2019);

            var (_, availabilities) = aggregate.MeasureAvailability();
            
            Assert.Equal(31, availabilities.Count());
            foreach (var item in availabilities)
            {
                Assert.NotEqual(-1, item.Minimun);
                Assert.NotEqual(-1, item.Maximun);
                Assert.NotEqual(-1, item.Average);
                Assert.NotEqual(-1, item.Availability);                
            }            
        }
    }
}

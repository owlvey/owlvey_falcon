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
        public void MeasureAvailability()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                Calendar.StartJanuary2019,
                Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");
            var sourceItemB = SourceItemEntity.Factory.Create(source,
                Calendar.StartJanuary2019, Calendar.EndJanuary2019,
                900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);
            source.SourceItems.Add(sourceItemB );

            var aggregate = new SourceAvailabilityAggregate(source,                
                Calendar.StartJanuary2019,
                Calendar.EndJanuary2019);

            var (_, availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, availabilities.Count());
            Assert.Equal(0.769m, availabilities.First().Availability);
        }

        [Fact]
        public void MeasureAvailabilityNotMinusOne()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");

            var sourceItemA = SourceItemEntity.Factory.Create(source,
                Calendar.StartJuly2019,
                Calendar.EndJuly2019,
                3900223, 3911869, DateTime.Now, "test");

            source.SourceItems.Add(sourceItemA);

            var aggregate = new SourceAvailabilityAggregate(source,
                Calendar.StartJuly2019,
                Calendar.EndJuly2019);

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

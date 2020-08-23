using System;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class JourneyDailyAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {
            var (_, product, journey, feature) = TestDataFactory.BuildCustomerProductJourneyFeature();
            var source = TestDataFactory.BuildSource(product);
            var sourceA = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItem = SourceEntity.Factory.CreateItem(source,
                OwlveyCalendar.StartJanuary2019,
                900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);

            var indicatorA = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItemA = SourceEntity.Factory.CreateItem(source, 
                OwlveyCalendar.EndJanuary2019, 900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);

            source.SourceItems.Add(sourceItem);
            indicator.Source = source;

            source.SourceItems.Add(sourceItemA);
            indicatorA.Source = sourceA;

            feature.Indicators.Add(indicator);
            feature.Indicators.Add(indicatorA);

            var journey_aggregate = new JourneyDailyAggregate(journey,
                new DatePeriodValue(OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019));

            var (journey_availabilities, _) = journey_aggregate.MeasureQuality;

            Assert.Equal(2, journey_availabilities.Count());

        }
    }
}

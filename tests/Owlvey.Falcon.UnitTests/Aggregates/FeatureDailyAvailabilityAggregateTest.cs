using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using TDF = Owlvey.Falcon.UnitTests.TestDataFactory;
using static Owlvey.Falcon.UnitTests.TestDataFactory;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class FeatureDailyAvailabilityAggregateTest
    {

        [Fact]
        public void MeasureFeatureAvailability() {

            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);
            var sourceA = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItems = SourceEntity.Factory.CreateInteractionsFromRange(source, OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            foreach (var item in sourceItems)
            {
                source.SourceItems.Add(item);
            }            

            var indicatorA = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItemsA = SourceEntity.Factory.CreateInteractionsFromRange(source, OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            foreach (var item in sourceItemsA)
            {
                sourceA.SourceItems.Add(item);
            }            

            feature.Indicators.Add(indicator);
            feature.Indicators.Add(indicatorA);

            var aggregate = new FeatureDailyAvailabilityAggregate(feature,                
                new Core.Values.DatePeriodValue(
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019));

            var (features_availabilities, _) = aggregate.MeasureAvailability();

            Assert.Equal(31, features_availabilities.Count());
        }


        [Fact]
        public void MeasureFeatureAvailabilityEmpyIndicators()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            
            var aggregate = new FeatureDailyAvailabilityAggregate(feature, 
                new DatePeriodValue( OwlveyCalendar.StartJanuary2017, OwlveyCalendar.EndJanuary2017));

            var (features_availabilities, _) = aggregate.MeasureAvailability();

            Assert.Empty(features_availabilities);
        }
    }
}

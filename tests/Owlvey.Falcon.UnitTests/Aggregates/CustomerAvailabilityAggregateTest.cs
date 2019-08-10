using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using TDF = Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class CustomerAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);
            var sourceA = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItem = SourceItemEntity.Factory.Create(source, TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItem);            

            var indicatorA = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItemA = SourceItemEntity.Factory.Create(source, TDF.Calendar.StartJanuary2019,
                TDF.Calendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            sourceA.SourceItems.Add(sourceItemA);

            feature.Indicators.Add(indicator);
            feature.Indicators.Add(indicatorA);

            var aggregate = new FeatureAvailabilityAggregate(feature,                
                TDF.Calendar.StartJanuary2019, TDF.Calendar.EndJanuary2019);

            var (_, features_availabilities) = aggregate.MeasureAvailability();

            Assert.Equal(31, features_availabilities.Count());
        }
    }
}

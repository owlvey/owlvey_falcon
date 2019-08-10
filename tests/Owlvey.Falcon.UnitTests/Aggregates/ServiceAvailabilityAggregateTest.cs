using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class ServiceAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {
            var (_, product, service, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);
            var sourceA = TestDataFactory.BuildSource(product);

            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItem = SourceItemEntity.Factory.Create(source, Calendar.StartJanuary2019,
                Calendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            var indicatorA = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "/api/customer");

            var sourceItemA = SourceItemEntity.Factory.Create(source, Calendar.StartJanuary2019,
                Calendar.EndJanuary2019, 900, 1200, DateTime.Now, "test");

            source.SourceItems.Add(sourceItem);            
            indicator.Source = source;
                        
            source.SourceItems.Add(sourceItemA);
            indicatorA.Source = sourceA;
                        
            feature.Indicators.Add(indicator);
            feature.Indicators.Add(indicatorA);
                        
            var service_aggregate = new ServiceAvailabilityAggregate(service, 
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var (_, service_availabilities) = service_aggregate.MeasureAvailability();

            Assert.Equal(31, service_availabilities.Count());
            
        }
    }
}

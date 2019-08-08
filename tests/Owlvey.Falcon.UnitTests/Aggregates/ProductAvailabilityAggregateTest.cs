using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class ProductAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureProductAvailability()
        {
            var (_, product, service, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product);

            var (indicator_a, items_a) = TestDataFactory.Indicators.GenerateSourceItems(feature, source);
            var (indicator_b, items_b) = TestDataFactory.Indicators.GenerateSourceItems(feature, source);
                        
            var indicator_availability = new IndicatorAvailabilityAggregator(indicator_a, items_a,
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var indicator_availabilities = indicator_availability.MeasureAvailability();

            var indicator_availabilityA = new IndicatorAvailabilityAggregator(indicator_b, items_b,
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var indicatorA_availabilities = indicator_availabilityA.MeasureAvailability();

            var aggregate = new FeatureAvailabilityAggregate(feature,
                new[] { indicator_availabilities, indicatorA_availabilities },
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var features_availabilities = aggregate.MeasureAvailability();
            
            var service_aggregate = new ServiceAvailabilityAggregate(service, new[] { features_availabilities },
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var service_availabilities = service_aggregate.MeasureAvailability();


            var product_aggregate = new ProductAvailabilityAggregate(product, new[] { service_availabilities },
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var product_availabilities = product_aggregate.MeasureAvailability();

            Assert.Equal(31, product_availabilities.availabilities.Count());
        }
    }
}
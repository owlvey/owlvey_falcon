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
            

            var indicator_a = TestDataFactory.Indicators.GenerateSourceItems(product, feature);
            var indicator_b = TestDataFactory.Indicators.GenerateSourceItems(product, feature);

            feature.Indicators.Add(indicator_a);
            feature.Indicators.Add(indicator_b);
                        
            
            var service_aggregate = new ServiceAvailabilityAggregate(service, Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var service_availabilities = service_aggregate.MeasureAvailability();
            
            var product_aggregate = new ProductAvailabilityAggregate(product, 
                Calendar.StartJanuary2019, Calendar.EndJanuary2019);

            var product_availabilities = product_aggregate.MeasureAvailability();

            Assert.Equal(31, product_availabilities.availabilities.Count());
        }
    }
}
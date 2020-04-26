using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using System.Linq;
using static Owlvey.Falcon.UnitTests.TestDataFactory;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class ProductAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureProductAvailability()
        {
            var (_, product, service, feature) = BuildCustomerProductServiceFeature();
            

            var indicator_a = Indicators.GenerateSourceItems(product, feature);
            var indicator_b = Indicators.GenerateSourceItems(product, feature);

            feature.Indicators.Add(indicator_a);
            feature.Indicators.Add(indicator_b);
                        
            
            var service_aggregate = new ServiceDailyAvailabilityAggregate(service, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019);

            var service_availabilities = service_aggregate.MeasureAvailability();
            
            var product_aggregate = new ProductDailyAggregate(product, 
                new DatePeriodValue(OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJanuary2019));

            var product_availabilities = product_aggregate.MeasureQuality();
            Assert.Equal(2, product_availabilities.Count());
        }
    }
}
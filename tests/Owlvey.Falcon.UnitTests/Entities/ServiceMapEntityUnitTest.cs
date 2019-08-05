using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class ServiceMapEntityUnitTest
    {
        [Fact]
        public void CreateServiceMapSuccess() {
            var (customer, product, service, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var map = ServiceMapEntity.Factory.Create(service, feature, DateTime.Now, "test");
            Assert.NotNull(map.Feature);
            Assert.NotNull(map.Service);

        }
    }
}

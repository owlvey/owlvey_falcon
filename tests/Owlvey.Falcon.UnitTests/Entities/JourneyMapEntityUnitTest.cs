using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class JourneyMapEntityUnitTest
    {
        [Fact]
        public void CreateJourneyMapSuccess() {
            var (customer, product, journey, feature) = TestDataFactory.BuildCustomerProductJourneyFeature();
            var map = JourneyMapEntity.Factory.Create(journey, feature, DateTime.Now, "test");
            Assert.NotNull(map.Feature);
            Assert.NotNull(map.Journey);

        }
    }
}

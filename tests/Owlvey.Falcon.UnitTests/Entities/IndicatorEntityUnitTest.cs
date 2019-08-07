using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class IndicatorEntityUnitTest
    {
        [Fact]
        public void CreateIndicatorEntitySuccess()
        {
            var (_, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var source = TestDataFactory.BuildSource(product, "test", DateTime.Now, "createdby");
            var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, "test");
            Assert.NotNull(indicator.Source);
        }
    }
}

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
            var source = TestDataFactory.BuildSource("test", DateTime.Now, "createdby");
            feature.RegisterIndicator(source, DateTime.Now, "test");
            Assert.NotEmpty(feature.Indicators);
        }
    }
}

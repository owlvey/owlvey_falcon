using System;
using Owlvey.Falcon.Core;
using Xunit;

namespace Owlvey.Falcon.UnitTests
{
    public class QualityUtilsUnitTest
    {
        [Fact]
        public void TestProportionToNumbers10() {
            decimal target = 0.8M;
            var  (g, t) = QualityUtils.ProportionToNumbers(target);
            Assert.Equal(g, 80);
            Assert.Equal(t, 100); 
        }


        [Fact]
        public void TestProportionToNumbers100()
        {
            decimal target = 0.85M;
            var (g, t) = QualityUtils.ProportionToNumbers(target);
            Assert.Equal(g, 85);
            Assert.Equal(t, 100);
        }
    }
}

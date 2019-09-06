using System;
using Owlvey.Falcon.Core;
using Xunit;

namespace Owlvey.Falcon.UnitTests
{
    public class FilterUtilUnitTest
    {
        [Fact]
        public void ParseNoEqual() {
            var result = FilterUtils.ParseQuery("aline ne 1");
            Assert.Equal("aline", result.field);
            Assert.Equal(FilterOperator.ne, result.opera);
            Assert.Equal(1, int.Parse(result.value));
            Assert.True(true);
        }
    }
}

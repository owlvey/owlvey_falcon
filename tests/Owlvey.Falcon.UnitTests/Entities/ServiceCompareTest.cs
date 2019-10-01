using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class ServiceCompareTest
    {
        [Fact]
        public void CompareSuccess()
        {
            var compare = new ServiceCompare();

            var result = compare.Equals(new ServiceEntity() { Id = 2 }, new ServiceEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new ServiceEntity() { Id = 2 }, new ServiceEntity() { Id = 2 });
            Assert.True(result);
        }
    }
}

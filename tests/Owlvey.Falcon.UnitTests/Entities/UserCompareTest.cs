using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class UserCompareTest
    {
        [Fact]
        public void CompareSuccess()
        {
            var compare = new UserCompare();

            var result = compare.Equals(new UserEntity() { Id = 2 }, new UserEntity() { Id = 3 });
            Assert.False(result);

            result = compare.Equals(new UserEntity() { Id = 2 }, new UserEntity() { Id = 2 });
            Assert.True(result);
        }
    }
}

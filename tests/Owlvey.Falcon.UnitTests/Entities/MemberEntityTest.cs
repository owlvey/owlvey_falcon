using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class MemberEntityTest
    {
        [Fact]
        public void CraeteMemberEntity()
        {
            var member = MemberEntity.Factory.Create(1, 2, DateTime.Now, "test");
            Assert.NotNull(member.CreatedBy);
        }
    }
}

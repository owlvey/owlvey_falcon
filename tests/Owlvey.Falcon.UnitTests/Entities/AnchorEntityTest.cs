using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class AnchorEntityTest
    {
        [Fact]
        public void CreateAnchorSuccess() {

            var (_, product)= TestDataFactory.BuildCustomerProduct();            
            var entity = AnchorEntity.Factory.Create("test", DateTime.Now, "test", product);

            Assert.Equal("test", entity.Name);

            entity.Update(OwlveyCalendar.January201903, DateTime.Now, "test");

            Assert.Equal(OwlveyCalendar.January201903, entity.Target);

        }
    }
}

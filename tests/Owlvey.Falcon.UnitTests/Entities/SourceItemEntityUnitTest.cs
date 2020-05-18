using Owlvey.Falcon.Core.Entities;
using System;
using System.Linq;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SourceItemEntityUnitTest
    {
        [Fact]
        public void CreateProductEntitySuccess()
        {            
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var source = TestDataFactory.BuildSource(product);
            var sourceItem = SourceEntity.Factory.CreateInteraction(source, new DateTime(2019, 01, 01, 8,0,0,0),
                 900, 1000, DateTime.Now, "test");
            Assert.NotNull(sourceItem.Source);                       
        }


        [Fact]
        public void CraeteRangeSourceItems() {
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var source = TestDataFactory.BuildSource(product);
            var sourceItems = SourceEntity.Factory.CreateInteractionsFromRange(source,
                new DateTime(2019, 01, 01),
                new DateTime(2019, 01, 31),
                 900, 1000, DateTime.Now, "test");
            Assert.Equal(31, sourceItems.Count());

        }

        [Fact]
        public void CraeteRangeSourceItems4Days()
        {
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var source = TestDataFactory.BuildSource(product);
            var sourceItems = SourceEntity.Factory.CreateInteractionsFromRange(source,
                new DateTime(2019, 01, 02),
                new DateTime(2019, 01, 05),
                 900, 1000, DateTime.Now, "test");
            Assert.Equal(4, sourceItems.Count());

        }
    }
}

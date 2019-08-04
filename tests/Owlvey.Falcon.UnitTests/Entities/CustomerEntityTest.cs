using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class CustomerEntityTest
    {
        [Fact]
        public void CreateCustomerSuccess()
        {
            var entity = CustomerEntity.Factory.Create("test", DateTime.Now, "test");
            Assert.NotNull(entity.CreatedBy);
            Assert.NotNull(entity.CreatedOn);
            Assert.NotNull(entity.ModifiedBy);
            Assert.NotNull(entity.ModifiedOn);
            Assert.NotNull(entity.Name);
        }

        [Fact]
        public void AddProductToCustomerSuccess()
        {
            var entity = CustomerEntity.Factory.Create("test", DateTime.Now, "test");
            var product = ProductEntity.Factory.Create("test", DateTime.Now);
            entity.AddProduct(product);
            Assert.NotEmpty(entity.Products);
        }
    }
}

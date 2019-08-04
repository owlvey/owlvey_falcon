using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class CustomerEntityUnitTest
    {
        public CustomerEntityUnitTest()
        {

        }

        [Fact]
        public void CreateCustomerEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Faker.Company.Name();            

            var customerEntity = CustomerEntity.Factory.Create(createdBy, DateTime.UtcNow, name);

            Assert.Equal(name, customerEntity.Name);
            Assert.Equal(createdBy, customerEntity.CreatedBy);
        }

        [Fact]
        public void CreateCustomerEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;

            Assert.Throws<InvalidStateException>(() =>
            {
                CustomerEntity.Factory.Create(name, DateTime.UtcNow, createdBy);
            });

        }

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
            var product = ProductEntity.Factory.Create("test", "test", DateTime.Now, "user" );
            entity.AddProduct(product);
            Assert.NotEmpty(entity.Products);
        }
    }
}

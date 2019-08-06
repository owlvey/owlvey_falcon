using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class ProductEntityUnitTest
    {

        [Fact]
        public void CreateProductEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");            

            var customer = TestDataFactory.BuildCustomer();

            var productEntity = ProductEntity.Factory.Create(name, DateTime.UtcNow, createdBy, customer);

            Assert.Equal(name, productEntity.Name);            
            Assert.Equal(createdBy, productEntity.CreatedBy);
        }

        [Fact]
        public void CreateProductEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;

            var customer = TestDataFactory.BuildCustomer();

            Assert.Throws<InvalidStateException>(() =>
            {
                ProductEntity.Factory.Create(name, DateTime.UtcNow, createdBy, customer);
            });

        }
    }
}

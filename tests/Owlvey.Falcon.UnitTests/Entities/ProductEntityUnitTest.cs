using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class ProductEntityUnitTest
    {
        public ProductEntityUnitTest()
        {

        }

        [Fact]
        public void CreateProductEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");
            var description = Guid.NewGuid().ToString("n");

            var productEntity = ProductEntity.Factory.Create(name, description, DateTime.UtcNow, createdBy);

            Assert.Equal(name, productEntity.Name);
            Assert.Equal(description, productEntity.Description);
            Assert.Equal(createdBy, productEntity.CreatedBy);
        }

        [Fact]
        public void CreateProductEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;

            Assert.Throws<InvalidStateException>(() =>
            {
                ProductEntity.Factory.Create(name, description, DateTime.UtcNow, createdBy);
            });

        }
    }
}

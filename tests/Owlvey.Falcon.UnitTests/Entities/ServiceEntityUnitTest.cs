using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class ServiceEntityUnitTest
    {
        [Fact]
        public void CreateServiceEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");
            decimal slo = 0.99m;

            var (_, product) = TestDataFactory.BuildCustomerProduct();
            
            var serviceEntity = ServiceEntity.Factory.Create(name, DateTime.UtcNow, createdBy, product);

            Assert.Equal(name, serviceEntity.Name);
            Assert.Equal(slo, serviceEntity.AvailabilitySlo);
            Assert.Equal(createdBy, serviceEntity.CreatedBy);
        }
        [Fact]
        public void AddServiceEntitySuccess() {
             var (customer, product) = TestDataFactory.BuildCustomerProduct();
            var service = TestDataFactory.BuildService("test", 95, "user", DateTime.Now);
            product.AddService(service);
            Assert.NotEmpty(product.Services);             
        }

        [Fact]
        public void CreateServiceEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;            
            var (_, product) = TestDataFactory.BuildCustomerProduct();

            Assert.Throws<InvalidStateException>(() =>
            {
                ServiceEntity.Factory.Create(name,  DateTime.UtcNow, createdBy, product);
            });

        }
    }
}

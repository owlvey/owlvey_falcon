using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
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

            var customerEntity = CustomerEntity.Factory.Create(name, createdBy);

            Assert.Equal(name, customerEntity.Name);
            Assert.Equal(createdBy, customerEntity.CreatedBy);
        }

        [Fact]
        public void CreateCustomerEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;

            Assert.Throws<ApplicationException>(() =>
            {
                CustomerEntity.Factory.Create(name, createdBy);
            });

        }
    }
}

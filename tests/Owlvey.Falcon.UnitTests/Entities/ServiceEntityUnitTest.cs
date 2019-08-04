using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class ServiceEntityUnitTest
    {
        public ServiceEntityUnitTest()
        {

        }

        [Fact]
        public void CreateServiceEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");
            var description = Guid.NewGuid().ToString("n");
            float slo = 99;

            var serviceEntity = ServiceEntity.Factory.Create(name, description, slo, createdBy);

            Assert.Equal(name, serviceEntity.Name);
            Assert.Equal(description, serviceEntity.Description);
            Assert.Equal(slo, serviceEntity.SLO);
            Assert.Equal(createdBy, serviceEntity.CreatedBy);
        }

        [Fact]
        public void CreateServiceEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;
            float slo = 0;

            Assert.Throws<ApplicationException>(() =>
            {
                ServiceEntity.Factory.Create(name, description, slo, createdBy);
            });

        }
    }
}

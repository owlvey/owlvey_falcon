using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class JourneyEntityUnitTest
    {
        [Fact]
        public void CreateJourneyEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = Guid.NewGuid().ToString("n");
            decimal slo = 0.99m;

            var (_, product) = TestDataFactory.BuildCustomerProduct();
            
            var journeyEntity = JourneyEntity.Factory.Create(name, DateTime.UtcNow, createdBy, product);

            Assert.Equal(name, journeyEntity.Name);
            Assert.Equal(slo, journeyEntity.AvailabilitySlo);
            Assert.Equal(createdBy, journeyEntity.CreatedBy);
        }
        [Fact]
        public void AddJourneyEntitySuccess() {
             var (customer, product) = TestDataFactory.BuildCustomerProduct();
            var journey = TestDataFactory.BuildJourney("test", 95, "user", DateTime.Now);
            product.AddJourney(journey);
            Assert.NotEmpty(product.Journeys);             
        }

        [Fact]
        public void CreateJourneyEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = string.Empty;
            var description = string.Empty;            
            var (_, product) = TestDataFactory.BuildCustomerProduct();

            Assert.Throws<InvalidStateException>(() =>
            {
                JourneyEntity.Factory.Create(name,  DateTime.UtcNow, createdBy, product);
            });

        }
    }
}

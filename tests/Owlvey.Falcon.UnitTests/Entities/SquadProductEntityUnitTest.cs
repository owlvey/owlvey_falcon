using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SquadProductEntityUnitTest
    {        
        [Fact]
        public void CreateSquadProductSuccess()
        {

            var (customer, product, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var squad = TestDataFactory.BuildSquad(customer);

            var entity = SquadProductEntity.Factory.Create(squad, product, DateTime.Now, "test@test.com");
            Assert.NotNull(entity.CreatedBy);
            Assert.NotNull(entity.CreatedOn);
            Assert.NotNull(entity.ModifiedBy);
            Assert.NotNull(entity.ModifiedOn);
            Assert.NotNull(entity.Product);
            Assert.NotNull(entity.Squad);
        }
    }
}

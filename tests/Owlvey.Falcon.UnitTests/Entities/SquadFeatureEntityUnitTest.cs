using System;
using Owlvey.Falcon.Core.Entities;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class SquadFeatureEntityUnitTest
    {        
        [Fact]
        public void CreateSquadFeatureSuccess()
        {

            var (customer, _, _, feature) = TestDataFactory.BuildCustomerProductServiceFeature();
            var squad = TestDataFactory.BuildSquad(customer);

            var entity = SquadFeatureEntity.Factory.Create(squad, feature, DateTime.Now, "test@test.com");
            Assert.NotNull(entity.CreatedBy);
            Assert.NotNull(entity.CreatedOn);
            Assert.NotNull(entity.ModifiedBy);
            Assert.NotNull(entity.ModifiedOn);
            Assert.NotNull(entity.Feature);
            Assert.NotNull(entity.Squad);
        }
    }
}

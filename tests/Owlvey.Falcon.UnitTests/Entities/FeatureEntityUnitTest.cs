using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class FeatureEntityUnitTest
    {
        public FeatureEntityUnitTest()
        {

        }

        [Fact]
        public void CreateFeatureEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = $"Feature A";

            var featureEntity = FeatureEntity.Factory.Create(name, DateTime.UtcNow, createdBy);

            Assert.Equal(name, featureEntity.Name);
            Assert.Equal(createdBy, featureEntity.CreatedBy);
        }

        [Fact]
        public void CreateFeatureEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = $"";

            Assert.Throws<InvalidStateException>(() =>
            {
                FeatureEntity.Factory.Create(name, DateTime.UtcNow, createdBy);
            });

        }

        [Fact]
        public void AddServiceEntitySuccess()
        {
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var feature = TestDataFactory.BuildFeature("test", "user", DateTime.Now);
            product.AddFeature(feature);
            Assert.NotEmpty(product.Features);
        }
    }
}

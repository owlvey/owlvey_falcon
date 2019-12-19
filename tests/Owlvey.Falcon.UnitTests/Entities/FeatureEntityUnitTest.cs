using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Exceptions;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class FeatureEntityUnitTest
    {
        

        [Fact]
        public void CreateFeatureEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = $"Feature A";            

            var (_, product) = TestDataFactory.BuildCustomerProduct();

            var featureEntity = FeatureEntity.Factory.Create(name, DateTime.UtcNow, createdBy, product);

            Assert.Equal(name, featureEntity.Name);
            Assert.Equal(createdBy, featureEntity.CreatedBy);
        }

        [Fact]
        public void UpdateFeatureSuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = $"Feature A";

            var (_, product) = TestDataFactory.BuildCustomerProduct();

            var featureEntity = FeatureEntity.Factory.Create(name, DateTime.UtcNow, createdBy, product);
            var value = Guid.NewGuid().ToString();
            var change = DateTime.Now;
            featureEntity.Update(change, value, value, value, value);

            Assert.Equal(change, featureEntity.ModifiedOn);
            Assert.Equal(value, featureEntity.Name);
            Assert.Equal(value, featureEntity.Description);
            Assert.Equal(value, featureEntity.Avatar);
        }

        [Fact]
        public void MeasureIncidents() {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = $"Feature A";

            var (_, product) = TestDataFactory.BuildCustomerProduct();

            var featureEntity = FeatureEntity.Factory.Create(name, DateTime.UtcNow, createdBy, product);

            featureEntity.RegisterIncident(new IncidentMapEntity() {
                 Incident  = new IncidentEntity() {
                      TTD = 10, TTE = 20, TTF = 30
                 }
            });

            Assert.Equal(10, featureEntity.MTTD);
            Assert.Equal(20, featureEntity.MTTE);
            Assert.Equal(30, featureEntity.MTTF);
            Assert.Equal(60, featureEntity.MTTM);

        }

        [Fact]
        public void CreateFeatureEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = $"";            

            var (_, product) = TestDataFactory.BuildCustomerProduct();
            Assert.Throws<InvalidStateException>(() =>
            {
                FeatureEntity.Factory.Create(name, DateTime.UtcNow, createdBy, product);
            });

        }

        [Fact]
        public void AddFeatureEntitySuccess()
        {
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var feature = TestDataFactory.BuildFeature("test", "user", DateTime.Now);
            product.AddFeature(feature);
            Assert.NotEmpty(product.Features);
        }



    }
}

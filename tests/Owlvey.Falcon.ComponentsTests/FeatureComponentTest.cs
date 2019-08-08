using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class FeatureComponentTest
    {
        [Fact]
        public async Task FeatureMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var featureComponent = container.GetInstance<FeatureComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();

            await featureComponent.CreateFeature(new Models.FeaturePostRp() {
                 Name="test",
                 Description = "test",
                 ProductId=productId                 
            });

            var features = await featureQueryComponent.GetFeatures(productId);
            Assert.NotEmpty(features);
            
        }
    }
}

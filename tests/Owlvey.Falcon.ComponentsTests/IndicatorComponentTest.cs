using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class IndicatorComponentTest
    {
        [Fact]
        public async Task IndicatorMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var source = await ComponentTestFactory.BuildSource(container, product);
            var feature = await ComponentTestFactory.BuildFeature(container, product);

            var component = container.GetInstance<IndicatorComponent>();

            await component.Create(new Models.IndicatorPostRp()
            {
                FeatureId = feature,
                SourceId = source                
            });

            var indicators = await component.GetByFeature(feature);
            
            Assert.NotEmpty(indicators);

            foreach (var item in indicators)
            {
                var  indicator = await component.GetById(item.Id);
                Assert.NotNull(indicator);
                Assert.NotNull(indicator.FeatureAvatar);
                Assert.NotNull(indicator.Feature);
                Assert.NotNull(indicator.Source);
                Assert.NotNull(indicator.SourceAvatar);
            }
        }

        [Fact]
        public async Task IndicatorDuplicateFail() {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var source = await ComponentTestFactory.BuildSource(container, product);
            var feature = await ComponentTestFactory.BuildFeature(container, product);

            var component = container.GetInstance<IndicatorComponent>();

            await component.Create(new Models.IndicatorPostRp()
            {
                FeatureId = feature,
                SourceId = source
            });

            
            var result = await component.Create(new Models.IndicatorPostRp()
            {
                FeatureId = feature,
                SourceId = source
            });

            Assert.True(result.HasConflicts());            
        }

    }
}

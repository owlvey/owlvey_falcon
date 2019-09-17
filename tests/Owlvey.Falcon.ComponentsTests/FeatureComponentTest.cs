using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;
using System.Linq;

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
                 ProductId=productId                 
            });            

            var features = await featureQueryComponent.GetFeatures(productId);
            Assert.NotEmpty(features);
            
        }

        [Fact]
        public async Task FeatureSeriesTest()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var products = await productQueryComponent.GetProducts(customer);

            product = products.ElementAt(0).Id;
            
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();
            var features = await featureQueryComponent.GetFeatures(product);

            var daily = await featureQueryComponent.GetDailySeriesById(features.ElementAt(0).Id,
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.StartJuly2019);

            Assert.Equal(163, daily.Series[0].Items.Count());
            Assert.Equal(163, daily.Series[1].Items.Count());
            

            Assert.NotEmpty(features);
        }
        [Fact]
        public async Task FeatureMaintenanceSquadSucces() {

            var container = ComponentTestFactory.BuildContainer();
            var (customer, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            
            var featureComponent = container.GetInstance<FeatureComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();

            var feature = await featureComponent.CreateFeature(new Models.FeaturePostRp()
            {
                Name = "test",                
                ProductId = productId
            });            

            var squad = await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            

            await featureComponent.RegisterSquad(new Models.SquadFeaturePostRp()
            {
                FeatureId = feature.Id,
                SquadId = squad.Id
            });

            var detail = await featureQueryComponent.GetFeatureByIdWithAvailability(feature.Id, OwlveyCalendar.January201903, OwlveyCalendar.January201906);

            Assert.NotEmpty(detail.Squads);

            var squadDetail = await squadQueryComponent.GetSquadById(squad.Id);

            Assert.NotEmpty(squadDetail.Features);                       

        }


    }
}

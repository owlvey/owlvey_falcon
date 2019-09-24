using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class IncidentComponentTest
    {
        [Fact]
        public async Task IncidentMaintenance()
        {
            var container = ComponentTestFactory.BuildContainer();

            var component = container.GetInstance<IncidentComponent>();

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var (result, _) = await component.Post( new Models.IncidentPostRp() {
                Key = "test01",
                ProductId = product, Title = "test" });

            var items = await component.GetByProduct(product);
            Assert.NotEmpty(items);

            result = await component.Put(result.Id, new Models.IncidentPutRp() {                 
                 Title = "change"                 
            });

            var tmp = await component.Get(result.Id);
            Assert.Equal("change", tmp.Title);

        }

        [Fact]
        public async Task IncidentAssociations()
        {
            var container = ComponentTestFactory.BuildContainer();

            var incidentComponent = container.GetInstance<IncidentComponent>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var featureQueryComponent  = container.GetInstance<FeatureQueryComponent>();

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var (result, _) = await incidentComponent.Post(new Models.IncidentPostRp()
            {
                Key = "test01",
                ProductId = product,
                Title = "test"
            });

            var items = await incidentComponent.GetByProduct(product);
            Assert.NotEmpty(items);                        

            var feature = await featureComponent.CreateFeature(new Models.FeaturePostRp() { Name = "test", ProductId = product });

            await incidentComponent.RegisterFeature(result.Id, feature.Id);

            var featureDetail = await featureQueryComponent.GetFeatureByIdWithAvailability(feature.Id, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.StartJanuary2020);

            Assert.NotEmpty(featureDetail.Incidents);
        }
    }
}

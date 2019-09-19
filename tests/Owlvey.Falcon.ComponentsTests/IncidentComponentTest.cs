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

            var result = await component.Post( new Models.IncidentPostRp() {
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
    }
}

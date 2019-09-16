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

            var result = await component.Post( new Models.IncidentPostRp() { ProductId = product, Title = "test" });

            var items = await component.Get(product, new PeriodValue(OwlveyCalendar.StartJanuary2019, OwlveyCalendar.StartJanuary2020));
            Assert.NotEmpty(items);

            result = await component.Put(new Models.IncidentPutRp() {
                 Id = result.Id,
                 Title = "change",
                  Description = "change"
            });

            var tmp = await component.Get(result.Id);
            Assert.Equal("change", tmp.Title);
            Assert.Equal("change", tmp.Description);

        }
    }
}

using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class LatencySourceComponentTest
    {

        [Fact]
        public async Task SourceGetById()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var component = container.GetInstance<SourceComponent>();
                                  

            Models.SourceGetListRp source = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product,                 
            });
            var result = await component.GetByIdWithDetail(source.Id, OwlveyCalendar.year2019);
            Assert.Equal(101, result.Quality.Latency);
        }
    }
}

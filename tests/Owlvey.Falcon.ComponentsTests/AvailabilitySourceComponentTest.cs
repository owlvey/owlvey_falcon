using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class AvailabilitySourceComponentTest
    {

        [Fact]
        public async Task SourceGetById()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var component = container.GetInstance<SourceComponent>();
            
            SourceGetListRp source = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product,                                
            });

            var result = await component.GetByIdWithDetail(source.Id, OwlveyCalendar.year2019);
            Assert.Equal(1, result.Quality.Availability);
        }

        [Fact]
        public async Task SourceItemsGetById()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var component = container.GetInstance<SourceComponent>();
            var availability = container.GetInstance<SourceItemComponent>();

            SourceGetListRp source = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product,                                
            });

            await availability.CreateAvailabilityItem(new SourceItemAvailabilityPostRp()
            {
                Start = OwlveyCalendar.StartJanuary2019,
                End = OwlveyCalendar.EndJanuary2019,
                Good = 800,
                Total = 1000,
                SourceId = source.Id
            });


            var result = await availability.GetAvailabilityItems(source.Id, OwlveyCalendar.year2019);
            Assert.NotEmpty(result);

        }
    }
}

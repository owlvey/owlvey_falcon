using Owlvey.Falcon.Components;
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
            var availability = container.GetInstance<AvailabilitySourceComponent>();

            Models.SourceGetListRp source = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product,
                Group = Core.Entities.SourceGroupEnum.Availability,
                Kind = Core.Entities.SourceKindEnum.Interaction
            });

            var result = await availability.GetInteractionById(source.Id, OwlveyCalendar.year2019);
            Assert.Equal(1, result.Proportion);


        }

        [Fact]
        public async Task SourceItemsGetById()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var component = container.GetInstance<SourceComponent>();
            var availability = container.GetInstance<AvailabilitySourceComponent>();

            Models.SourceGetListRp source = await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product,
                Group = Core.Entities.SourceGroupEnum.Availability,
                Kind = Core.Entities.SourceKindEnum.Interaction
            });

            await availability.CreateInteraction(new Models.SourceItemInteractionPostRp()
            {
                Start = OwlveyCalendar.StartJanuary2019,
                End = OwlveyCalendar.EndJanuary2019,
                Good = 800,
                Total = 1000,
                SourceId = source.Id
            });


            var result = await availability.GetInteractionItems(source.Id, OwlveyCalendar.year2019);
            Assert.NotEmpty(result);

        }
    }
}

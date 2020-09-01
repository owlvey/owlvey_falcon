using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class ReliabilityRiskComponentTest
    {
        [Fact]
        public async void Maintenance()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var sourceComponent = container.GetInstance<SourceComponent>();

            var source = await sourceComponent.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });
            var component = container.GetInstance<ReliabilityRiskComponent>();

            var risk = await  component.Create(new ReliabilityRiskPostRp() {
                 Name = "test", SourceId = source.Id
            });

            var risks = await component.GetRisks(source.Id);
            Assert.NotEmpty(risks);

            var target = await component.GetRiskById(risk.Id);
            Assert.NotNull(target);
        }
    }
}

using FizzWare.NBuilder.Extensions;
using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SecurityRiskComponentTest
    {
        [Fact]
        public async Task Maintentance() {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var sourceComponent = container.GetInstance<SourceComponent>();

            var source = await sourceComponent.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });

            var component = container.GetInstance<SecurityRiskComponent>();

            var threat = await component.CreateThreat(new Models.SecurityThreatPostRp() {
                 Name = "test"
            });

            var risk = await component.Create(new Models.SecurityRiskPost()
            {
                SourceId = source.Id                
            });

            var risks = await component.GetRisks(source.Id);

            Assert.NotEmpty(risks);

            var riskGet = await component.GetRiskById(risk.Id);
            Assert.NotNull(riskGet);

            await component.UpdateRisk(risk.Id, 
                new Models.SecurityRiskPut() { 
                  AgentSkillLevel = 3
            });

            riskGet = await component.GetRiskById(risk.Id);
            Assert.Equal(3, riskGet.AgentSkillLevel);

            await component.DeleteRisk(risk.Id);

            risks = await component.GetRisks(source.Id);
            Assert.Empty(risks);
        }
    }
}

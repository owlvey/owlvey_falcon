using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class ReliabilityThreatComponentTest
    {
        [Fact]
        public async void Maintenance() {
            var container = ComponentTestFactory.BuildContainer();

            var component = container.GetInstance<ReliabilityRiskComponent>();

            await component.CreateThreat(new ReliabilityThreatPostRp() { 
                 Name = "test"
            });

            var threats = await component.GetThreats();
            Assert.NotEmpty(threats);
        }
    }
}

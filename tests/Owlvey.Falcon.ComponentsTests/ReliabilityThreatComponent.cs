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
        public async void DefaultThreats(){
            var container = ComponentTestFactory.BuildContainer();
            var component = container.GetInstance<ReliabilityRiskComponent>();
            await component.CreateDefault();
            var threats = await component.GetThreats();
            foreach( var item in threats){
                Assert.False( String.IsNullOrWhiteSpace(item.Name));
                Assert.Equal( 10, item.ETTD);
                Assert.NotNull( item.Description );
            }
        }

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

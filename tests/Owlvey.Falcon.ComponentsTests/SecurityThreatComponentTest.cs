﻿using Owlvey.Falcon.Components;
using Owlvey.Falcon.ComponentsTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SecurityThreatComponentTest
    {

        [Fact]
        public async Task DefaultThreats(){
            var container = ComponentTestFactory.BuildContainer();
            var component = container.GetInstance<SecurityRiskComponent>();
            await component.CreateDefault();
            var threats = await component.GetThreats();
            Assert.NotEmpty(threats);            
        }
        [Fact]
        public async Task Maintentance() {
            var container = ComponentTestFactory.BuildContainer();

            var component = container.GetInstance<SecurityRiskComponent>();

            var threat = await component.CreateThreat(new Models.SecurityThreatPostRp() { 
                 Name = MockUtils.GenerateRandomName()
            });

            var threats = await component.GetThreats();
            Assert.NotEmpty(threats);

            var threatGet = await component.GetThreat(threat.Id);
            Assert.NotNull(threatGet);

            await component.UpdateThreat(threat.Id, new Models.SecurityThreatPutRp() { 
                 Name = "change"
            });

            threatGet = await component.GetThreat(threat.Id);
            Assert.Equal("change", threatGet.Name);

            await component.DeleteThreat(threat.Id);

            threatGet = await component.GetThreat(threat.Id);
            Assert.Null(threatGet);
        }
    }
}

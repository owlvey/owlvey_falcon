using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    
    public class SecurityThreatEntityTest
    {
        [Fact]
        public void MeasureSucess() {
            var threat = SecurityThreatEntity.Create("test", "test", DateTime.Now);
            threat.AgentSkillLevel = 4;
            Assert.Equal(1, threat.ThreatAgentFactor);
            Assert.Equal(0.5m, threat.LikeHood);
            threat.LossConfidentiality = 4;
            Assert.Equal(1, threat.TechnicalImpact);
            Assert.Equal(0.5m, threat.Impact);
            Assert.Equal(0.25m, threat.Risk);
        }
    }
}

using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    
    public class ReliabilityThreatEntityTest
    {
        [Fact]
        public void MaintenanceSucess() {
            var threat = ReliabilityThreatEntity.Factory.Create(DateTime.Now, "test name", "test");            
            Assert.Equal("test", threat.Name);
        }
    }
}

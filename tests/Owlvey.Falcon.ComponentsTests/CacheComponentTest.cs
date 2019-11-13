using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class CacheComponentTest
    {
        [Fact]
        public void CacheServicesSuccess() {
            var container = ComponentTestFactory.BuildContainer();
            var cache = container.GetInstance<CacheComponentA>();
            cache.SetServicesAvailability(1, new List<ServiceGetListRp>() { new ServiceGetListRp() });
            var result = cache.GetServicesAvailability(1);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}

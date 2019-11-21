﻿using System;
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
            var cache = container.GetInstance<CacheComponent>();
            var modified = cache.GetLastModified();
            Assert.NotNull(modified);
        }

        [Fact]
        public async Task LastUpdateVersionSuccess() {
            var container = ComponentTestFactory.BuildContainer();
            var cache = container.GetInstance<CacheComponent>();

            var mark = await cache.GetLastModified();
            var mark2 = await cache.GetLastModified();

            Assert.Equal(mark, mark2);

            var customerComponent = container.GetInstance<CustomerComponent>();
            await customerComponent.CreateCustomer(new CustomerPostRp()
            {
                Name = "test"
            });

            var mark3 = await cache.GetLastModified();

            Assert.NotEqual(mark, mark3);


        }
    }
}

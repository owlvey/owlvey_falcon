﻿using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class ProductComponentTest
    {

        [Fact]
        public async Task ProductIdempotenceTest()
        {
            var container = ComponentTestFactory.BuildContainer();

            var customerId = await ComponentTestFactory.BuildCustomer(container);

            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            var result = await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });
            var result2 = await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            Assert.Equal(result.Id, result2.Id);
        }

        [Fact]
        public async Task ProductMaintenanceSuccess() {

            var container = ComponentTestFactory.BuildContainer();

            var customerId = await ComponentTestFactory.BuildCustomer(container);
                        
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            var products = await productQueryComponent.GetProducts(customerId);

            Assert.NotEmpty(products);

        }
    }
}

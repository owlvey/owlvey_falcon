using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class CustomerComponentTest
    {
        [Fact]
        public async Task SimpleCustomerSetupSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();

            await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test",
                Avatar = "default"
            });

            var customers = await customerQueryComponent.GetCustomers();

            Assert.NotEmpty(customers);

            var customer = await customerQueryComponent.GetCustomerByName("test");

            Assert.NotNull(customer);

            customer = await customerQueryComponent.GetCustomerById(customer.Id);

            Assert.NotNull(customer);
            
            await customerComponet.UpdateCustomer(customer.Id, new Models.CustomerPutRp() { Name = "change", Avatar = "change" });

            customer = await customerQueryComponent.GetCustomerById(customer.Id);

            Assert.Equal("change", customer.Name);

            await customerComponet.DeleteCustomer(customer.Id);

            customer = await customerQueryComponent.GetCustomerById(customer.Id);

            Assert.Null(customer);

        }

    }
}

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
        public async Task CustomerIdempotenceTest()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
            });

            var result2 = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
            });

            Assert.Equal(result.Id, result2.Id);
        }

        [Fact]
        public async Task MaintenanceCustomerScucess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();

            var customer = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
            });            

            await customerComponet.DeleteCustomer(customer.Id);

            var result = await customerQueryComponent.GetCustomerById(customer.Id);

            Assert.Null(result);

        }

        [Fact]
        public async Task SimpleCustomerSetupSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();

            await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"                
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

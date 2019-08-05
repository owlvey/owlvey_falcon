using Microsoft.EntityFrameworkCore;
using Moq;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Data.SQLite.Context;
using Owlvey.Falcon.Data.SQLite.Repositories;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class CustomerComponentTest
    {
        [Fact]
        public async void SimpleCustomerSetupSuccess()
        {
            var options = new DbContextOptionsBuilder<FalconDbContext>().UseInMemoryDatabase(databaseName: "Owlvey").Options;
            using (var context = new FalconDbContext(options))
            {
                var mockIdentity = new Mock<IUserIdentityGateway>();

                mockIdentity.Setup(c => c.GetIdentity()).Returns("test");

                var repository = new CustomerRepository(context);
                var customerComponet = new CustomerComponent(context, mockIdentity.Object);
                var customerQueryComponent = new CustomerQueryComponent(context); 

                await customerComponet.CreateCustomer(new Models.CustomerPostRp()
                {
                    Name = "test"
                });

                var customers = await customerQueryComponent.GetCustomers();

                Assert.NotEmpty(customers);                 
            }            
        }

    }
}

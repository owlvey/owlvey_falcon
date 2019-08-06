using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Interfaces;
using Owlvey.Falcon.Repositories;
using SimpleInjector;

namespace Owlvey.Falcon.ComponentsTests
{
    public static class ComponentTestFactory
    {
        public static Container BuildContainer(){
            Container container = new Container();
            var mapper = BuildMapper();
            container.RegisterInstance<IMapper>(mapper);
            container.RegisterInstance<IUserIdentityGateway>(BuildIdentityGateway());
            container.RegisterInstance<IDateTimeGateway>(BuildDateTimeGateway());
            container.RegisterInstance<FalconDbContext>(new FalconDbContextInMemory());
            return container;
        }

        public static IMapper BuildMapper() {
            var configuration = new MapperConfiguration(cfg =>
            {
                CustomerComponentConfiguration.ConfigureMappers(cfg);
                ProductComponentConfiguration.ConfigureMappers(cfg);
                FeatureComponentConfiguration.ConfigureMappers(cfg);
                ServiceComponentConfiguration.ConfigureMappers(cfg);
                UserComponentConfiguration.ConfigureMappers(cfg);
                SquadComponentConfiguration.ConfigureMappers(cfg);
            });
            configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();
            return mapper;
        }
        public static IUserIdentityGateway BuildIdentityGateway() {
            var mockIdentity = new Mock<IUserIdentityGateway>();
            mockIdentity.Setup(c => c.GetIdentity()).Returns("component_test_user");
            return mockIdentity.Object;
        }

        public static IDateTimeGateway BuildDateTimeGateway() {
            return new DateTimeGateway();
        }


        public static async Task<int> BuildCustomer(Container container, string name = "test") {            
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();
            await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = name
            });
            var customer = await customerQueryComponent.GetCustomerByName(name);
            return customer.Id;
        }

        public static async Task<int> BuildUser(Container container, string email = "test@test.com")
        {
            var userComponent = container.GetInstance<UserComponent>();
            
            await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = email
            });
            var user = await userComponent.GetUserByEmail(email);
            return user.Id;
        }

        public static async Task<(int customer, int product)> BuildProduct(Container container,
            string customerName="customer", string productName="product") {

            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();
            await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = customerName
            });

            var productComponent = container.GetInstance<ProductComponent>();
            var customer = await customerQueryComponent.GetCustomerByName(customerName);
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            await productComponent.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customer.Id,
                Name = productName
            });

            var product = await productQueryComponent.GetProductByName(productName);

            return (customer.Id, product.Id);
        }

    }
}

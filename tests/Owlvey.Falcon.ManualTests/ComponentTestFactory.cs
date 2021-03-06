﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Repositories;
using SimpleInjector;

namespace Owlvey.Falcon.ManualTests
{
    public static class ComponentTestFactory
    {
        public static Container BuildContainer()
        {
            Container container = new Container();
            var mapper = BuildMapper();
            container.RegisterInstance<IMapper>(mapper);
            container.RegisterInstance<IUserIdentityGateway>(BuildIdentityGateway());
            container.RegisterInstance<IDateTimeGateway>(BuildDateTimeGateway());
            var builder = new DbContextOptionsBuilder<FalconDbContext>();
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString).AddInterceptors(new OwlveyCommandInterceptor());

            container.RegisterInstance<FalconDbContext>(new FalconDbContext(builder.Options));
            return container;
        }

        public static IMapper BuildMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                CustomerComponentConfiguration.ConfigureMappers(cfg);
                ProductComponentConfiguration.ConfigureMappers(cfg);
                FeatureComponentConfiguration.ConfigureMappers(cfg);
                IncidentComponentConfiguration.ConfigureMappers(cfg);
                JourneyComponentConfiguration.ConfigureMappers(cfg);
                UserComponentConfiguration.ConfigureMappers(cfg);
                SquadComponentConfiguration.ConfigureMappers(cfg);
                MemberComponentConfiguration.ConfigureMappers(cfg);
                SourceComponentConfiguration.ConfigureMappers(cfg);
                SourceItemComponentConfiguration.ConfigureMappers(cfg);
                IndicatorComponentConfiguration.ConfigureMappers(cfg);
                SourceComponent.ConfigureMappers(cfg);
                SecurityRiskComponent.ConfigureMappers(cfg);
                ReliabilityRiskComponent.ConfigureMappers(cfg);
                MigrationComponent.ConfigureMappers(cfg);
            });
            configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();
            return mapper;
        }
        public static IUserIdentityGateway BuildIdentityGateway()
        {
            var mockIdentity = new Mock<IUserIdentityGateway>();
            mockIdentity.Setup(c => c.GetIdentity()).Returns("component_test_user");
            return mockIdentity.Object;
        }

        public static IDateTimeGateway BuildDateTimeGateway()
        {
            return new DateTimeGateway();
        }


        public static async Task<int> BuildCustomer(Container container, string name = "test")
        {
            var customerComponet = container.GetInstance<CustomerComponent>();
            var customerQueryComponent = container.GetInstance<CustomerQueryComponent>();
            await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = name
            });
            var customer = await customerQueryComponent.GetCustomerByName(name);
            return customer.Id;
        }

        public static async Task<(int customer, int squad)> BuildCustomerSquad(Container container, string name = "test")
        {

            var customer = await ComponentTestFactory.BuildCustomer(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squad = await squadQueryComponent.GetSquadByName(customer, "test");

            return (customer, squad.Id);
        }

        public static async Task<int> BuildUser(Container container, string email = "test@test.com")
        {
            var userComponent = container.GetInstance<UserComponent>();
            var userQueryComponent = container.GetInstance<UserQueryComponent>();

            await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = email
            });
            var user = await userQueryComponent.GetUserByEmail(email);
            return user.Id;
        }

        public static async Task<int> BuildJourney(Container container, int? product = null, string name = "journey")
        {
            if (!product.HasValue)
            {
                var (_, product_id) = await BuildCustomerProduct(container);
                product = product_id;
            }

            var journeyComponent = container.GetInstance<JourneyComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();
            await journeyComponent.Create(new Models.JourneyPostRp() { Name = name, ProductId = product.Value});
            var journey = await journeyQueryComponent.GetByProductIdName(product.Value, name);
            return journey.Id;
        }

        public static async Task<int> BuildSource(Container container, int? product = null, string name = "journey")
        {
            if (!product.HasValue)
            {
                var (_, product_id) = await BuildCustomerProduct(container);
                product = product_id;
            }
            var component = container.GetInstance<SourceComponent>();
            await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product.Value
            });

            var target = await component.GetByName(product.Value, "test");

            return target.Id;
        }

        public static async Task<int> BuildFeature(Container container, int? product = null, string name = "feature")
        {
            if (!product.HasValue)
            {
                var (_, product_id) = await BuildCustomerProduct(container);
                product = product_id;
            }

            var component = container.GetInstance<FeatureComponent>();
            var queryComponent = container.GetInstance<FeatureQueryComponent>();
            await component.CreateFeature(new Models.FeaturePostRp() { Name = name, ProductId = product.Value });
            var feature = await queryComponent.GetFeatureByName(product.Value, name);
            return feature.Id;
        }

        public static async Task<(int customer, int product)> BuildCustomerProduct(Container container,
            string customerName = "customer", string productName = "product")
        {

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

            var product = await productQueryComponent.GetProductByName(customer.Id, productName);

            return (customer.Id, product.Id);
        }


    }
}

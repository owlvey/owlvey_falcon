﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using SimpleInjector;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.ComponentsTests.Mocks;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.ComponentsTests
{
    public static class OwlveyCalendar
    {

        public static DateTime StartJanuary2017 = new DateTime(2017, 01, 01);
        public static DateTime EndJanuary2017 = new DateTime(2017, 12, 31);
        public static DateTime StartJanuary2019 = new DateTime(2019, 01, 01);
        public static DateTime StartJanuary2020 = new DateTime(2020, 01, 01);
        public static DateTime January201903 = new DateTime(2019, 01, 03);
        public static DateTime January201904 = new DateTime(2019, 01, 04);
        public static DateTime January201905 = new DateTime(2019, 01, 05);
        public static DateTime January201906 = new DateTime(2019, 01, 06);
        public static DateTime January201907 = new DateTime(2019, 01, 07);
        public static DateTime January201908 = new DateTime(2019, 01, 08);
        public static DateTime January201910 = new DateTime(2019, 01, 10);
        public static DateTime January201912 = new DateTime(2019, 01, 12);
        public static DateTime January201914 = new DateTime(2019, 01, 14);
        public static DateTime January201920 = new DateTime(2019, 01, 20);
        public static DateTime EndJanuary2019 = new DateTime(2019, 01, 31);
        public static DateTime EndDecember2019 = new DateTime(2019, 12, 31);

        public static DateTime StartJuly2019 = new DateTime(2019, 07, 1);
        public static DateTime EndJuly2019 = new DateTime(2019, 07, 31);
        public static DatePeriodValue year2019 = new DatePeriodValue(StartJanuary2019, EndDecember2019);

    }
    public static class ComponentTestFactory
    {
        public static Container BuildContainer(){
            Container container = new Container();
            var mapper = BuildMapper();
            container.RegisterInstance<IMapper>(mapper);
            container.RegisterInstance<IUserIdentityGateway>(BuildIdentityGateway());
            container.Register<IDateTimeGateway, MockDateTimeGateway>();            
            
            var context = new FalconDbContextInMemory();

            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            //context.Migrate("Development");
            context.SeedData("Development", new MockDateTimeGateway().GetCurrentDateTime());            
            container.RegisterInstance<FalconDbContext>(context);
            return container;
        }

        public static IMapper BuildMapper() {
            var configuration = new MapperConfiguration(cfg =>
            {
                CustomerComponentConfiguration.ConfigureMappers(cfg);
                ProductComponentConfiguration.ConfigureMappers(cfg);
                FeatureComponentConfiguration.ConfigureMappers(cfg);
                IncidentComponentConfiguration.ConfigureMappers(cfg);
                ServiceComponentConfiguration.ConfigureMappers(cfg);
                UserComponentConfiguration.ConfigureMappers(cfg);
                SquadComponentConfiguration.ConfigureMappers(cfg);
                MemberComponentConfiguration.ConfigureMappers(cfg);
                SourceComponentConfiguration.ConfigureMappers(cfg);
                SourceItemComponentConfiguration.ConfigureMappers(cfg);
                SquadFeatureComponentConfiguration.ConfigureMappers(cfg);                
                IndicatorComponentConfiguration.ConfigureMappers(cfg);

                LatencySourceComponent.ConfigureMappers(cfg);
                AvailabilitySourceComponent.ConfigureMappers(cfg);
                ExperienceSourceComponent.ConfigureMappers(cfg);
                SourceComponent.ConfigureMappers(cfg);
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

        public static async Task<int> BuildService(Container container, int? product= null, string name = "service")
        {
            if (!product.HasValue) {
                var (_, product_id) = await BuildCustomerProduct(container);
                product = product_id;
            }

            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();
            await serviceComponent.CreateService(new Models.ServicePostRp() { Name = name, ProductId = product.Value });
            var service = await serviceQueryComponent.GetServiceByName(product.Value, name);
            return service.Id;
        }

        public static async Task<int> BuildSource(Container container, int? product = null, string name = "service")
        {
            if (!product.HasValue)
            {
                var (_, product_id) = await BuildCustomerProduct(container);
                product = product_id;
            }
            var component = container.GetInstance<SourceComponent>();
            await component.Create(new Models.SourcePostRp()
            {
                Name = name,
                ProductId = product.Value
            });

            var target = await component.GetByName(product.Value, name);

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
            await component.CreateFeature(new Models.FeaturePostRp() { Name = name,  ProductId = product.Value });
            var feature = await queryComponent.GetFeatureByName(product.Value, name);
            return feature.Id;
        }

        public static async Task<(int customer, int product)> BuildCustomerProduct(Container container,
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

            await productComponent.CreateProduct(new ProductPostRp()
            {
                CustomerId = customer.Id,
                Name = productName
            });

            var product = await productQueryComponent.GetProductByName(customer.Id, productName);

            return (customer.Id, product.Id);
        }

        public static async Task<(int customerId, int productId, int serviceId, int featureId, 
            int sourceId, int squadId)> BuildCustomerWithSquad(Container container,
            DateTime start, DateTime end) {

            var squadComponent = container.GetInstance<SquadComponent>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var serviceComponent = container.GetInstance<ServiceComponent>();
            var serviceMapComponent = container.GetInstance<ServiceMapComponent>();
            var indicatorComponent = container.GetInstance<IndicatorComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            
            var service = await serviceComponent.CreateService(new Models.ServicePostRp()
            {
                Name = "test service",
                ProductId = product
            });

            var feature = await featureComponent.CreateFeature(new Models.FeaturePostRp()
            {
                Name = "test",
                ProductId = product
            });

            await serviceMapComponent.CreateServiceMap(new ServiceMapPostRp()
            {
                FeatureId = feature.Id,
                ServiceId = service.Id
            });

            var source = await sourceComponent.Create(new Models.SourcePostRp()
            {
                Name = "test source",
                ProductId = product
            });

            await sourceItemComponent.Create(new Models.SourceItemInteractionPostRp()
            {
                SourceId = source.Id,
                Start = start,
                End = end,
                Total = 1000,
                Good = 800
            });

            await indicatorComponent.Create(feature.Id, source.Id);

            var squad = await squadComponent.CreateSquad(new Models.SquadPostRp() { Name = "test", CustomerId = customer });

            await featureComponent.RegisterSquad(new Models.SquadFeaturePostRp() { FeatureId = feature.Id, SquadId = squad.Id });

            return (customer, product, service.Id, feature.Id, source.Id, squad.Id);
        }
    }
}

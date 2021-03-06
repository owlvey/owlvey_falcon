﻿using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.UnitTests
{
    public static class TestDataFactory
    {
        public static CustomerEntity BuildCustomer(bool defaultValue = true) {
            var entity = CustomerEntity.Factory.Create("test", DateTime.Now, "test", defaultValue : defaultValue);
            return entity;
        }
        public static ProductEntity BuildProduct(CustomerEntity entity = null)
        {
            var customer = entity ?? BuildCustomer();
            var createdBy = Guid.NewGuid().ToString("n");
            var name = "test product";
            var productEntity = ProductEntity.Factory.Create(name, DateTime.UtcNow, createdBy, customer);
            return productEntity;
        }
        public static SquadEntity BuildSquad(CustomerEntity entity = null)
        {
            var customer = entity ?? BuildCustomer();
            var createdBy = Guid.NewGuid().ToString("n");
            var name = "test squad";
            var squad = SquadEntity.Factory.Create(name, DateTime.UtcNow, createdBy, customer);
            return squad;
        }
        public static JourneyEntity BuildJourney(string name, decimal slo, string createdBy, DateTime on) {
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var entity = JourneyEntity.Factory.Create(name, on, createdBy, product);
            return entity;
        }
        public static FeatureEntity BuildFeature(string name, string createdBy, DateTime on)
        {
            var (_, product) = TestDataFactory.BuildCustomerProduct();
            var entity = FeatureEntity.Factory.Create(name, on, createdBy, product);
            return entity;
        }
        public static SourceEntity BuildSource(ProductEntity product, string name = "/owlvey", DateTime? on = null, string createdBy = "test") {

            on = on ?? DateTime.Now;
            var entity = SourceEntity.Factory.Create(product, name, on.Value, createdBy);
            return entity;
        }

        public static (CustomerEntity, ProductEntity) BuildCustomerProduct() {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct(customer);
            return (customer, product);
        }

        public static (CustomerEntity, ProductEntity, JourneyEntity) BuildCustomerProductJourney()
        {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct(customer);
            var journey = TestDataFactory.BuildJourney("test", 99, "test", DateTime.Now);
            product.AddJourney(journey);

            return (customer, product, journey);
        }

        public static (CustomerEntity, ProductEntity, JourneyEntity, FeatureEntity) BuildCustomerProductJourneyFeature()
        {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct(customer);
            var journey = TestDataFactory.BuildJourney("test", 99, "test", DateTime.Now);
            var feature = TestDataFactory.BuildFeature("test", "test", DateTime.Now);
            var map = JourneyMapEntity.Factory.Create(journey, feature, DateTime.Now, "test");
            product.AddJourney(journey);
            product.AddFeature(feature);
            journey.FeatureMap.Add(map); 
            
            return (customer, product, journey, feature);
        }

        public static ICollection<SourceEntity> BuildSources(ProductEntity product) {
            var results = new List<SourceEntity>();
            var userName = "john doe";
            var tags = "load balancer";
            var source = TestDataFactory.BuildSource(product, "GET:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource(product, "POST:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource(product, "PUT:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            return results;
        }
        public static ICollection<SourceEntity> BuildSourcesWithItems(ProductEntity product)
        {
            var results = new List<SourceEntity>();
            var userName = "john doe";
            var tags = "load balancer";
            var source = TestDataFactory.BuildSource(product, "GET:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource(product, "POST:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource(product, "PUT:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            var start = new DateTime(2019, 01, 01);            
            foreach (var item in results)
            {
                SourceEntity.Factory.CreateItem(item, start, 900, 1000, DateTime.Now, "test", SourceGroupEnum.Availability);
            }
            return results;
        }

        public static class OwlveyCalendar{

            public static DateTime StartJanuary2017 = new DateTime(2017, 01, 01);
            public static DateTime EndJanuary2017 = new DateTime(2017, 12, 31);
            public static DateTime StartJanuary2019 = new DateTime(2019, 01, 01);
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

            public static DateTime StartJuly2019 = new DateTime(2019, 07, 1);
            public static DateTime EndJuly2019 = new DateTime(2019, 07, 31);

        }


        public static class Indicators {
            public static IndicatorEntity GenerateSourceItems(ProductEntity product, FeatureEntity feature) {
                var source = TestDataFactory.BuildSource(product);
                
                var indicator = IndicatorEntity.Factory.Create(feature, source, DateTime.Now, Guid.NewGuid().ToString());

                var sourceItem = SourceEntity.Factory.CreateItem(source, OwlveyCalendar.StartJanuary2019,
                    900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);                

                var sourceItemA = SourceEntity.Factory.CreateItem(source, 
                    OwlveyCalendar.EndJanuary2019, 900, 1200, DateTime.Now, "test", SourceGroupEnum.Availability);

                source.SourceItems.Add(sourceItem);
                source.SourceItems.Add(sourceItemA);

                indicator.Source = source;

                return indicator;
            }
        }

    }
}

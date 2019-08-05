using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.UnitTests
{
    public static class TestDataFactory
    {
        public static CustomerEntity BuildCustomer() {
            var entity = CustomerEntity.Factory.Create("test", DateTime.Now, "test");
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

        public static ServiceEntity BuildService(string name, float slo, string createdBy, DateTime on) {            
            var entity = ServiceEntity.Factory.Create(name, name, slo, on, createdBy);
            return entity;
        }
        public static FeatureEntity BuildFeature(string name, string createdBy, DateTime on)
        {
            var entity = FeatureEntity.Factory.Create(name, on, createdBy);
            return entity;
        }
        public static SourceEntity BuildSource(string name="/owlvey", DateTime? on=null, string createdBy="test") {

            on = on ?? DateTime.Now;
            var entity = SourceEntity.Factory.Create(name, on.Value, createdBy);
            return entity;
        }

        public static (CustomerEntity, ProductEntity) BuildCustomerProduct() {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct(customer);
            return (customer, product);
        }

        public static (CustomerEntity, ProductEntity, ServiceEntity) BuildCustomerProductService()
        {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct(customer);
            var service = TestDataFactory.BuildService("test", 99, "test", DateTime.Now);            
            product.AddService(service);

            return (customer, product, service);
        }

        public static (CustomerEntity, ProductEntity, ServiceEntity, FeatureEntity) BuildCustomerProductServiceFeature()
        {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct(customer);
            var service = TestDataFactory.BuildService("test", 99, "test", DateTime.Now);
            var feature = TestDataFactory.BuildFeature("test", "test", DateTime.Now);            
            product.AddService(service);
            product.AddFeature(feature); 
            return (customer, product, service, feature);
        }

        public static ICollection<SourceEntity> BuildSources() {
            var results = new List<SourceEntity>();
            var userName = "john doe";
            var tags = "load balancer";
            var source = TestDataFactory.BuildSource("GET:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource("POST:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource("PUT:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            return results;
        }
        public static ICollection<SourceEntity> BuildSourcesWithItems()
        {
            var results = new List<SourceEntity>();
            var userName = "john doe";
            var tags = "load balancer";
            var source = TestDataFactory.BuildSource("GET:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource("POST:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            source = TestDataFactory.BuildSource("PUT:/owlvey/api/customers", DateTime.Now, userName);
            source.Tags = tags;
            results.Add(source);
            var start = new DateTime(2019, 01, 01);
            var end = new DateTime(2019, 01, 02);
            foreach (var item in results)
            {
                SourceItemEntity.Factory.Create(item, start, end, 900, 1000, DateTime.Now, "test");
            }
            return results;
        }


    }
}

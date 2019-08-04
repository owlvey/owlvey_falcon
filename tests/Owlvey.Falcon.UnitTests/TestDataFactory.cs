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
        public static ProductEntity BuildProduct()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var name = "test product";
            var description = "test description";
            var productEntity = ProductEntity.Factory.Create(name, description, DateTime.UtcNow, createdBy);
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

        public static (CustomerEntity, ProductEntity) BuildCustomerProduct() {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct();
            customer.AddProduct(product);
            return (customer, product);
        }

        public static (CustomerEntity, ProductEntity, ServiceEntity) BuildCustomerProductService()
        {
            var customer = TestDataFactory.BuildCustomer();
            var product = TestDataFactory.BuildProduct();
            var service = TestDataFactory.BuildService("test", 99, "test", DateTime.Now);
            customer.AddProduct(product);
            product.AddService(service);

            return (customer, product, service);
        }


    }
}

using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories;
using Xunit;
using System.Linq;

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


        [Fact]
        public async Task ProductIdempotenceSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();

            var customerId = await ComponentTestFactory.BuildCustomer(container);
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            var products = await productQueryComponent.GetProducts(customerId);
            Assert.NotEmpty(products);
            Assert.Equal(2, products.Count());
        }


        [Fact]
        public async Task ProductDeleteSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var dbcontext = container.GetInstance<FalconDbContext>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var indicatorComponent = container.GetInstance<IndicatorComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var serviceComponent = container.GetInstance<ServiceComponent>();
            var productComponent = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var serviceQueryComponent = container.GetInstance<ServiceQueryComponent>();
            var data = await ComponentTestFactory.BuildCustomerWithSquad(container,
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);
            
            await productComponent.DeleteProduct(data.productId);

            var service = await productQueryComponent.GetProductById(data.productId);
            Assert.Null(service);
            var feature = await featureQueryComponent.GetFeatureById(data.featureId);
            Assert.Null(feature);
            var map = dbcontext.ServiceMaps.Where(c => c.ServiceId == data.serviceId).ToList();
            Assert.Empty(map);
        }

        [Fact]
        public async Task ProductReportDaily() {
            var container = ComponentTestFactory.BuildContainer();
            var customerId = await ComponentTestFactory.BuildCustomer(container);
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();


            var product = (await productQueryComponent.GetProducts(customerId)).ElementAt(0);            

            var result = await productQueryComponent.GetDailyServiceSeriesById(product.Id, OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.January201910);

            Assert.NotEmpty(result.Series);
        }

        [Fact]
        public async Task GetProductExportToExcel() {
            var container = ComponentTestFactory.BuildContainer();
            var customerId = await ComponentTestFactory.BuildCustomer(container);
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            var product = await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            var result = await productQueryComponent.GetProductExportToExcel(product.Id, OwlveyCalendar.StartJanuary2017,
                OwlveyCalendar.EndJanuary2019);

            Assert.NotNull(result.Item1);
        }

        [Fact]
        public async Task AnchorMaintenanceSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerId = await ComponentTestFactory.BuildCustomer(container);
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            var product = await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            var anchor = await productQueryComponent.GetAnchor(product.Id, "sample");
            Assert.NotNull(anchor);

            await productComponet.PutAnchor(product.Id, "sample", new Models.AnchorPutRp() { Target = DateTime.Now });

        }

        #region dashboard

        [Fact]
        public async Task OperationDashboard()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerId = await ComponentTestFactory.BuildCustomer(container);
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            var product = await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            var result = await productQueryComponent.GetProductDashboard(product.Id, OwlveyCalendar.January201903,
                OwlveyCalendar.EndJuly2019);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ServiceGroupDashboard()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customerId = await ComponentTestFactory.BuildCustomer(container);
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();

            var product = await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = "test"
            });

            var result = await productQueryComponent.GetServiceGroupDashboard(product.Id, OwlveyCalendar.January201903,
                OwlveyCalendar.EndJuly2019);

            Assert.NotNull(result); 
        }
        #endregion


    }
}

using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories;
using Xunit;
using System.Linq;
using Owlvey.Falcon.Core.Values;
using Moq;
using Owlvey.Falcon.ComponentsTests.Mocks;

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
                Name = MockUtils.GenerateRandomName()
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

            var name = MockUtils.GenerateRandomName();
            await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = name
            });

            await productComponet.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customerId,
                Name = name
            });

            var products = await productQueryComponent.GetProducts(customerId);
            Assert.NotEmpty(products);
            Assert.Single(products);
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
            var journeyComponent = container.GetInstance<JourneyComponent>();
            var productComponent = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var journeyQueryComponent = container.GetInstance<JourneyQueryComponent>();
            var data = await ComponentTestFactory.BuildCustomerWithSquad(container,
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);
            
            await productComponent.DeleteProduct(data.productId);

            var journey = await productQueryComponent.GetProductById(data.productId);
            Assert.Null(journey);
            var feature = await featureQueryComponent.GetFeatureById(data.featureId);
            Assert.Null(feature);
            var map = dbcontext.JourneyMaps.Where(c => c.JourneyId == data.journeyId).ToList();
            Assert.Empty(map);
        }

        [Fact]
        public async Task ProductReportDaily() {
            var container = ComponentTestFactory.BuildContainer();
            var customerId = await ComponentTestFactory.BuildCustomer(container, defaultValue: true );
            var productComponet = container.GetInstance<ProductComponent>();
            var productQueryComponent = container.GetInstance<ProductQueryComponent>();


            var product = (await productQueryComponent.GetProducts(customerId)).ElementAt(0);            

            var result = await productQueryComponent.GetDailyJourneysSeriesById(product.Id, 
                new DatePeriodValue( OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.January201910));

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
        public async Task JourneyGroupDashboard()
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

            var result = await productQueryComponent.GetJourneyGroupDashboard(product.Id, OwlveyCalendar.January201903,
                OwlveyCalendar.EndJuly2019);

            Assert.NotNull(result); 
        }
        #endregion


    }
}

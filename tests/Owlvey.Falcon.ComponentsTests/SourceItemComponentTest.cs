using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder.Extensions;
using OfficeOpenXml.FormulaParsing.Utilities;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.ComponentsTests.Mocks;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SourceItemComponentTest
    {
        [Fact]
        public async Task SourceItemBatchInteractionsEmpty()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customer = MockUtils.GenerateRandomName();
            var product = MockUtils.GenerateRandomName();

            var (_, _) = await ComponentTestFactory.BuildCustomerProduct(container,
                 customerName: customer, productName: product);
            var itemComponent = container.GetInstance<SourceItemComponent>();

            var items = await itemComponent.CreateInteractionItems(new SourceItemBatchPostRp()
            {
                Customer = customer,
                Product = product,
                Kind = Core.Entities.SourceKindEnum.Interaction,
                Items = new List<string>() {                     
                }
            });

            Assert.Equal(0, items.SourceCreated);
            Assert.Equal(0, items.ItemsCreated);
        }
        //OnboardingController::verifyOTPCode;2020-03-01 01:00:00;2020-03-01 01:59:59;75.0;0;59.0;1267.0053999999996

        [Fact]
        public async Task SourceItemBatchInteractionsCase()
        {
            var container = ComponentTestFactory.BuildContainer();
            var customer = MockUtils.GenerateRandomName();
            var product = MockUtils.GenerateRandomName();
            var context = container.GetInstance<FalconDbContext>();

            var (_, productId) = await ComponentTestFactory.BuildCustomerProduct(container,
                 customerName: customer, productName: product);
            var itemComponent = container.GetInstance<SourceItemComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();

            var items = await itemComponent.CreateInteractionItems(new SourceItemBatchPostRp()
            {
                Customer = customer,
                Product = product,
                Kind = Core.Entities.SourceKindEnum.Interaction,
                Delimiter = ';',
                Items = new List<string>() {
                     "OnboardingController::verifyOTPCode;2020-03-01 01:00:00;2020-03-01 01:59:59;75;0;59;1267.0053999999996",
                     "OnboardingController::verifyOTPCode;2020-03-01 02:00:00;2020-03-01 02:59:59;38;0;36;576.58925",
                     "OnboardingController::verifyOTPCode;2020-03-01 03:00:00;2020-03-01 03:59:59;22;0;21;571.0088000000001"
                 }
            });
            Assert.Equal(1, items.SourceCreated);
            Assert.Equal(9, items.ItemsCreated);
        }
        [Fact]
        public async Task SourceItemBatchInteractions() {
            var container = ComponentTestFactory.BuildContainer();
            var customer = MockUtils.GenerateRandomName();
            var product = MockUtils.GenerateRandomName();
            var context = container.GetInstance<FalconDbContext>();

            var ( _, productId) = await ComponentTestFactory.BuildCustomerProduct(container,
                 customerName: customer, productName: product );            
            var itemComponent = container.GetInstance<SourceItemComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();

            var items = await itemComponent.CreateInteractionItems(new SourceItemBatchPostRp() { 
                 Customer = customer,  Product = product, Kind = Core.Entities.SourceKindEnum.Interaction,
                 Items = new List<string>() {
                     "Controller::sendAdvisor,2020-03-03 01:00:00,2020-03-03 01:59:59,24,22,20,14164.86935",
                     "Controller::sendAdvisor,2020-03-04 01:00:00,2020-03-04 01:59:59,24,23,21,14164.86935",
                     "Controller::getAdvisorInfo,2020-03-03 00:00:00,2020-03-03 23:59:59,200,196,195,18474.299049999998"
                 }
            });
            Assert.Equal(2, items.SourceCreated);
            Assert.Equal(9, items.ItemsCreated);

            var sourceSendAdvisor = await sourceComponent.GetByName(productId, "Controller::sendAdvisor");
            Assert.NotNull(sourceSendAdvisor);
            var period = new DatePeriodValue(DateTime.Parse("2020-03-03 00:00:00"),
                    DateTime.Parse("2020-03-03 23:59:59"));
            var itemsSendAdvisor = await itemComponent.GetAvailabilityItems(sourceSendAdvisor.Id,
                    period
                );

            
            var all = context.SourcesItems.Where(c => c.SourceId == sourceSendAdvisor.Id && c.Group ==  Core.Entities.SourceGroupEnum.Availability
                    && c.Target >= period.Start && c.Target <= period.End).ToList();
            Assert.Equal(0.917m, itemsSendAdvisor.First().Measure);


            var itemsSendAdvisorExperience = await itemComponent.GetExperienceItems(sourceSendAdvisor.Id,
                    new DatePeriodValue(
                        DateTime.Parse("2020-03-03 00:00:00"),
                        DateTime.Parse("2020-03-03 23:59:59")
                    )
                );
            Assert.Equal(0.833m, itemsSendAdvisorExperience.First().Measure);

            var itemsSendAdvisorLatency= await itemComponent.GetLatencyItems(sourceSendAdvisor.Id,
                    new DatePeriodValue(DateTime.Parse("2020-03-03 00:00:00"),
                    DateTime.Parse("2020-03-03 23:59:59"))
                );
            Assert.Equal(14164.86935m, itemsSendAdvisorLatency.First().Measure);
        }

        [Fact]
        public async Task SourceItemStartMiddle() {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var source = await ComponentTestFactory.BuildSource(container, product: product);

            var sourceComponent = container.GetInstance<SourceComponent>();
            var itemComponent = container.GetInstance<SourceItemComponent>();
            
            await itemComponent.CreateAvailabilityItem(new SourceItemAvailabilityPostRp()
            {
                SourceId = source,
                Start = OwlveyCalendar.January201905,
                End = OwlveyCalendar.January201910,
                Good = 900,
                Total = 1000
            });
            var items = await itemComponent.GetBySourceIdAndDateRange(source, OwlveyCalendar.StartJanuary2019, OwlveyCalendar.January201908);
            Assert.NotEmpty(items);             
        }

        [Fact]
        public async Task SourceItemEndMiddle()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);
            var source = await ComponentTestFactory.BuildSource(container, product: product);

            var sourceComponent = container.GetInstance<SourceComponent>();
            var itemComponent = container.GetInstance<SourceItemComponent>();

            await itemComponent.CreateAvailabilityItem(new SourceItemAvailabilityPostRp()
            {
                SourceId = source,
                Start = OwlveyCalendar.January201905,
                End = OwlveyCalendar.January201910,
                Good = 900,
                Total = 1000
            });
            var items = await itemComponent.GetBySourceIdAndDateRange(source, OwlveyCalendar.January201908, OwlveyCalendar.January201912);
            Assert.NotEmpty(items);
        }

        [Fact]
        public async Task SourceItemMaintenanceSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();

            await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });

            var source = await component.GetByName(product, "test");

            var itemComponent = container.GetInstance<SourceItemComponent>();

            await itemComponent.CreateAvailabilityItem(new SourceItemAvailabilityPostRp()
            {
                 SourceId = source.Id,
                 Start = DateTime.Now,
                 End = DateTime.Now,
                 Good = 900,
                 Total = 1000                
            });

            var list = await itemComponent.GetBySource(source.Id);

            Assert.NotEmpty(list);

        }

        [Fact]
        public async Task SourceQueryAvailability()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var component = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();

            await component.Create(new Models.SourcePostRp()
            {
                Name = "test",
                ProductId = product
            });
            var testSource = await component.GetByName(product, "test");

            var item = await sourceItemComponent.CreateAvailabilityItem(new SourceItemAvailabilityPostRp()
            {
                SourceId = testSource.Id,
                Total = 1000,
                Good = 800,
                Start = OwlveyCalendar.January201904,
                End = OwlveyCalendar.January201906
            });

            var result = await sourceItemComponent.GetBySourceIdAndDateRange(testSource.Id, OwlveyCalendar.January201903, OwlveyCalendar.January201907);
            Assert.NotEmpty(result);

            result = await sourceItemComponent.GetBySourceIdAndDateRange(testSource.Id, OwlveyCalendar.January201905, OwlveyCalendar.January201907);       
            Assert.NotEmpty(result);

        }
    }
}

﻿using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;
using System.Linq;
using Owlvey.Falcon.Repositories;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.ComponentsTests
{
    public class FeatureComponentTest
    {
        [Fact]
        public async Task FeatureMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (_, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var featureComponent = container.GetInstance<FeatureComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();

            await featureComponent.CreateFeature(new Models.FeaturePostRp() {
                 Name="test",                 
                 ProductId=productId                 
            });            

            var features = await featureQueryComponent.GetFeatures(productId);
            Assert.NotEmpty(features);
            
        }
        [Fact]
        public async Task FeatureDeleteSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var dbcontext = container.GetInstance<FalconDbContext>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();
            var data = await ComponentTestFactory.BuildCustomerWithSquad(container,
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);

            await featureComponent.DeleteFeature(data.featureId);
            var feature = await featureQueryComponent.GetFeatureById(data.featureId);

            Assert.Null(feature);

            var indicators = dbcontext.Indicators.Where(c => c.FeatureId == data.featureId).ToList();
            Assert.Empty(indicators);
        }


        [Fact]
        public async Task IndicatorDeleteSuccess()
        {
            var container = ComponentTestFactory.BuildContainer();
            var dbcontext = container.GetInstance<FalconDbContext>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var indicatorComponent = container.GetInstance<IndicatorComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var data = await ComponentTestFactory.BuildCustomerWithSquad(container,
                OwlveyCalendar.January201903, OwlveyCalendar.January201905);

            await indicatorComponent.Delete(data.featureId, data.sourceId);
            var indicators = dbcontext.Indicators.Where(c => c.FeatureId == data.featureId).ToList();
            Assert.Empty(indicators);

            var feature = featureQueryComponent.GetFeatureById(data.featureId);
            Assert.NotNull(feature);
            var source = sourceComponent.GetById(data.sourceId);
            Assert.NotNull(source);
        }
        [Fact]
        public async Task FeatureSeriesTest()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container, defaultValues:true);

            var productQueryComponent = container.GetInstance<ProductQueryComponent>();
            var products = await productQueryComponent.GetProducts(customer);

            product = products.ElementAt(0).Id;
            
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();
            var features = await featureQueryComponent.GetFeatures(product);

            var daily = await featureQueryComponent.GetDailyAvailabilitySeriesById(features.ElementAt(0).Id,
                OwlveyCalendar.StartJanuary2019,
                OwlveyCalendar.StartJuly2019);

            Assert.Equal(163, daily.Series[0].Items.Count());
            Assert.Equal(163, daily.Series[1].Items.Count());            

            Assert.NotEmpty(features);
        }


        [Fact]
        public async Task FeatureDetail()
        {
            var container = ComponentTestFactory.BuildContainer();
            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container, defaultValues: true);

            var sourceComponent = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var indicatorComponent = container.GetInstance<IndicatorComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();

            var feature = await featureComponent.CreateFeature(new FeaturePostRp() { 
                Name = "test",  ProductId = product  });

            var source = await sourceComponent.Create(new SourcePostRp() { Name = "testSource", ProductId = product });

            await sourceItemComponent.CreateAvailabilityItem(new SourceItemAvailabilityPostRp() { 
                  Start = OwlveyCalendar.January201903,
                  End= OwlveyCalendar.January201903,
                  SourceId = source.Id, 
                  Total = 1000, 
                  Good = 800
            });

            await sourceItemComponent.CreateLatencyItem(new SourceItemLatencyPostRp()
            {
                Start = OwlveyCalendar.January201903,
                End = OwlveyCalendar.January201903,
                SourceId = source.Id,
                Measure = 1500                
            });

            await sourceItemComponent.CreateExperienceItem(new SourceItemExperiencePostRp()
            {
                Start = OwlveyCalendar.January201903,
                End = OwlveyCalendar.January201903,
                SourceId = source.Id,
                Measure = 0.9m
            });

            await indicatorComponent.Create(feature.Id, source.Id);

            var featureResult = await featureQueryComponent.GetFeatureByIdWithQuality(feature.Id, OwlveyCalendar.january2019); 
                        
            Assert.NotEmpty(featureResult.Indicators);
            Assert.Equal(0.8m, featureResult.Availability);
            Assert.Equal(0.8m, featureResult.Indicators.First().Measure.Availability);
            
            Assert.Equal(1000, featureResult.Indicators.First().Measure.Total);
            Assert.Equal(800, featureResult.Indicators.First().Measure.Good);

            Assert.Equal(1500m, featureResult.Latency);
            Assert.Equal(1500m, featureResult.Indicators.First().Measure.Latency);

            Assert.Equal(0.9m, featureResult.Experience);
            Assert.Equal(0.9m, featureResult.Indicators.First().Measure.Experience);
        }


        [Fact]
        public async Task FeatureMaintenanceSquadSucces() {

            var container = ComponentTestFactory.BuildContainer();
            var (customer, productId) = await ComponentTestFactory.BuildCustomerProduct(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            
            var featureComponent = container.GetInstance<FeatureComponent>();
            var featureQueryComponent = container.GetInstance<FeatureQueryComponent>();

            var feature = await featureComponent.CreateFeature(new Models.FeaturePostRp()
            {
                Name = "test",                
                ProductId = productId
            });            

            var squad = await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            

            await featureComponent.RegisterSquad(new Models.SquadFeaturePostRp()
            {
                FeatureId = feature.Id,
                SquadId = squad.Id
            });

            var detail = await featureQueryComponent.GetFeatureByIdWithQuality(feature.Id, 
                new DatePeriodValue( OwlveyCalendar.January201903, OwlveyCalendar.January201906));

            Assert.NotEmpty(detail.Squads);

            var squadDetail = await squadQueryComponent.GetSquadById(squad.Id);

            Assert.NotEmpty(squadDetail.Features);                       

        }


    }
}

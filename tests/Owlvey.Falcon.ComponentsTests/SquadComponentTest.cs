using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.ComponentsTests
{
    public class SquadComponentTest
    {
        [Fact]
        public async Task SquadMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var customer = await ComponentTestFactory.BuildCustomer(container);
            
            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squads = await squadQueryComponent.GetSquads(customer);            

            Assert.NotEmpty(squads);
        }

        [Fact]
        public async Task SquadFeatureMaintenanceSucces() {
            var container = ComponentTestFactory.BuildContainer();

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var featureComponent = container.GetInstance<FeatureComponent>(); 
            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            var squadResponse = await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squad = await squadQueryComponent.GetSquadById(squadResponse.Id);                        

            var feature = await ComponentTestFactory.BuildFeature(container, product);

            await featureComponent.RegisterSquad(new Models.SquadFeaturePostRp()
            {
                SquadId = squad.Id,
                FeatureId = feature
            });

            squad = await squadQueryComponent.GetSquadById(squad.Id);

            Assert.NotEmpty(squad.Features);

            await featureComponent.UnRegisterSquad(squad.Id, feature);

            squad = await squadQueryComponent.GetSquadById(squad.Id);

            Assert.Empty(squad.Features);

        }


        [Fact]
        public async Task SquadgetDetailSucces() {
            var container = ComponentTestFactory.BuildContainer();
            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            var squads = await squadQueryComponent.GetSquads(customer);

            var result = await squadQueryComponent.GetSquadByIdWithAvailability(squads.First().Id,
                OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJuly2019);
            
            Assert.NotEmpty(result.Features);
        }

        [Fact]
        public async Task SquadListSuccess() {
            var container = ComponentTestFactory.BuildContainer();
            var squadComponent = container.GetInstance<SquadComponent>();
            var featureComponent = container.GetInstance<FeatureComponent>();
            var serviceComponent= container.GetInstance<ServiceComponent>();
            var serviceMapComponent = container.GetInstance<ServiceMapComponent>();
            var indicatorComponent = container.GetInstance<IndicatorComponent>();
            var sourceComponent = container.GetInstance<SourceComponent>();
            var sourceItemComponent = container.GetInstance<SourceItemComponent>();            
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);


            var service  = await serviceComponent.CreateService(new Models.ServicePostRp() {
                 Name = "test service", ProductId = product
            });
                       
            var feature = await featureComponent.CreateFeature(new Models.FeaturePostRp() {
                 Name = "test", ProductId = product
            });

            await serviceMapComponent.CreateServiceMap(new ServiceMapPostRp()
            {
                FeatureId = feature.Id,
                ServiceId = service.Id
            });

            var source = await sourceComponent.Create(new Models.SourcePostRp() {
                 Name = "test source", ProductId = product
            });

            await sourceItemComponent.Create(new Models.SourceItemPostRp() {
                 SourceId = source.Id,
                 Start = OwlveyCalendar.January201903,
                 End = OwlveyCalendar.January201905,
                 Total = 1000,
                 Good = 800
            });

            await indicatorComponent.Create(feature.Id, source.Id);

            var squad = await squadComponent.CreateSquad(new Models.SquadPostRp() { Name = "test", CustomerId = customer });

            await featureComponent.RegisterSquad(new Models.SquadFeaturePostRp() { FeatureId = feature.Id, SquadId = squad.Id });

            var points = await squadQueryComponent.GetSquadByIdWithAvailability(squad.Id, OwlveyCalendar.January201903, OwlveyCalendar.January201905);

            Assert.Equal(-113.81m, points.Points); 
        }

        [Fact]
        public async Task SquadUserMaintenanceSucces()
        {
            var container = ComponentTestFactory.BuildContainer();

            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            var squadResponse = await squadComponent.CreateSquad(new Models.SquadPostRp()
            {
                Name = "test",
                CustomerId = customer
            });

            var squad =  await squadQueryComponent.GetSquadById(squadResponse.Id);

            var userComponent = container.GetInstance<UserComponent>();
            var userQueryComponent = container.GetInstance<UserQueryComponent>();

            var response = await userComponent.CreateUser(new Models.UserPostRp()
            {
                Email = "test@test.com"
            });

            var id = response.GetResult<int>("Id");

            var user = await userQueryComponent.GetUserById(id);

            await squadComponent.RegisterMember(squad.Id, user.Id);
            
            squad = await squadQueryComponent.GetSquadById(squad.Id);

            Assert.NotEmpty(squad.Members);

            await squadComponent.UnRegisterMember(squad.Id, user.Id);

            squad = await squadQueryComponent.GetSquadById(squad.Id);

            Assert.Empty(squad.Members);           
        
        }
    }
}

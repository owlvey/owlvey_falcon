using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Values;

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
            var (customer, product) = await ComponentTestFactory.BuildCustomerProduct(container, defaultValues: true);

            var squadComponent = container.GetInstance<SquadComponent>();
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();

            var squads = await squadQueryComponent.GetSquads(customer);

            var result = await squadQueryComponent.GetSquadByIdWithQuality(squads.First().Id,
                new DatePeriodValue(OwlveyCalendar.StartJanuary2019, OwlveyCalendar.EndJuly2019));
            
            Assert.NotEmpty(result.Features);
        }

        [Fact]
        public async Task SquadListSuccess() {
            var container = ComponentTestFactory.BuildContainer();            
            var squadQueryComponent = container.GetInstance<SquadQueryComponent>();
            var (_, _, _,_, _, squad) = await ComponentTestFactory.BuildCustomerWithSquad(container, 
                OwlveyCalendar.January201903, 
                OwlveyCalendar.January201905);
            
            var points = await squadQueryComponent.GetSquadByIdWithQuality(
                squad,                 
                new DatePeriodValue(
                OwlveyCalendar.January201903, 
                OwlveyCalendar.January201905));

            Assert.Equal(0.191m, points.Debt.Availability); 
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

            var id = response.Id;

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

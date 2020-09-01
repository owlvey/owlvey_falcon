using System;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Xunit;

namespace Owlvey.Falcon.ManualTests
{
    public class FinancialTestCase
    {
        [Fact]
        public async Task FinancialTestCaseOne()
        {
            var container = ComponentTestFactory.BuildContainer();

            var comUsers = container.GetInstance<UserComponent>();
            var comSquad = container.GetInstance<SquadComponent>();
            var comQuerySquad = container.GetInstance<SquadQueryComponent>();
            var comQueryUsers = container.GetInstance<UserQueryComponent>();
            var comMembers = container.GetInstance<MemberComponent>();
            var comQueryMembers = container.GetInstance<MemberQueryComponent>();
            var comCustomer = container.GetInstance<CustomerComponent>();
            var comQueryCustomer = container.GetInstance<CustomerQueryComponent>();
            var comProduct = container.GetInstance<ProductComponent>();
            var comQueryProduct = container.GetInstance<ProductQueryComponent>();
            var comJourney = container.GetInstance<JourneyComponent>();
            var comJourneyQueryComponent = container.GetInstance<JourneyQueryComponent>();
            var comFeature = container.GetInstance<FeatureComponent>();
            var comQueryFeature = container.GetInstance<FeatureQueryComponent>();
            var comJourneyMap = container.GetInstance<JourneyMapComponent>();
            var comSource = container.GetInstance<SourceComponent>();
            var comItemSource = container.GetInstance<SourceItemComponent>();
            var comIndicators = container.GetInstance<IndicatorComponent>();


            await comCustomer.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "Bank"
            });
            var customer = await comQueryCustomer.GetCustomerByName("Bank");

            await comSquad.CreateSquad(new Models.SquadPostRp()
            {
                CustomerId = customer.Id,
                Name = "team owlvey"
            });

            var squad = await comQuerySquad.GetSquadByName(customer.Id, "team owlvey");

            var users = new[] { "gregory", "ricardo", "felipe", "gustavo" };
            foreach (var item in users)
            {
                await comUsers.CreateUser(new Models.UserPostRp()
                {
                    Email = item + "@owlvey.com"
                });

                var user = await comQueryUsers.GetUserByEmail(item + "@owlvey.com");

                #region register members

                await comMembers.CreateMember(new Models.MemberPostRp()
                {
                    UserId = user.Id,
                    SquadId = squad.Id
                });

                #endregion
            }

            await comProduct.CreateProduct(new Models.ProductPostRp()
            {
                CustomerId = customer.Id,
                Name = "Application"
            });

            var product = await comQueryProduct.GetProductByName( customer.Id, "Application");

            var biz_names = new[] { "Login", "Onboarding", "Password Recovery",
                "Mesa de cambio", "Mi Lista", "Pago de Servicios", "Pago de Tarjeta de Credito",
                "Pago Prestamo", "Gates", "Perfil", "Limites", "Clave Digital", "Puntos", "Configuracion de tarjetas",
                "Transferencias", "Retiro sin tarjeta", "Dashboard", "Recargas", "Personal Financial Managment",
                "Detalle Producto" };

            foreach (var item in biz_names)
            {
                await comJourney.Create(new Models.JourneyPostRp()
                {
                    Name = item,
                    ProductId = product.Id                    
                });

                await comFeature.CreateFeature(new Models.FeaturePostRp()
                {
                    Name = item,                    
                    ProductId = product.Id
                });

                var journey = await comJourneyQueryComponent.GetByProductIdName(product.Id, item);
                var feature = await comQueryFeature.GetFeatureByName(product.Id, item);
                await comJourneyMap.CreateMap(new Models.JourneyMapPostRp()
                {
                    FeatureId = feature.Id,
                    JourneyId = journey.Id
                });

                await comSource.Create(new Models.SourcePostRp()
                {
                    Name = item + "source",
                    ProductId = product.Id
                });

                var source = await comSource.GetByName(product.Id, item + "source");

                await comIndicators.Create(feature.Id, source.Id);                
            }
        }
    }
}

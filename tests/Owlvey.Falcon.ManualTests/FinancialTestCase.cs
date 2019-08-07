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
            var comService = container.GetInstance<ServiceComponent>();
            var comQueryService = container.GetInstance<ServiceQueryComponent>();
            var comFeature = container.GetInstance<FeatureComponent>();
            var comQueryFeature = container.GetInstance<FeatureQueryComponent>();
            var comServiceMap = container.GetInstance<ServiceMapComponent>();
            var comSource = container.GetInstance<SourceComponent>();
            var comItemSource = container.GetInstance<SourceItemComponent>(); 
            var comIndicators = container.GetInstance<IndicatorComponent>();  


            await comCustomer.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "Bank"
            });
            var customer = await comQueryCustomer.GetCustomerByName("Bank");

            await comSquad.CreateSquad(new Models.SquadPostRp() {
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
                    UserId =  user.Id,
                    SquadId = squad.Id
                });

                #endregion
            }

            await comProduct.CreateProduct(new Models.ProductPostRp() {
                 CustomerId = customer.Id,
                 Name = "Application"
            });

            var product = await comQueryProduct.GetProductByName("Application");

            var biz_names = new[] { "Login", "Onboarding", "Password Recovery",
                "Mesa de cambio", "Mi Lista", "Pago de Servicios", "Pago de Tarjeta de Credito",
                "Pago Prestamo", "Gates", "Perfil", "Limites", "Clave Digital", "Puntos", "Configuracion de tarjetas",
                "Transferencias", "Retiro sin tarjeta", "Dashboard", "Recargas", "Personal Financial Managment",
                "Detalle Producto" };

            foreach (var item in biz_names)
            {
                await comService.CreateService(new Models.ServicePostRp()
                {
                     Name = item,
                     ProductId = product.Id,
                     Description = item,
                     SLO = 99
                });

                await comFeature.CreateFeature(new Models.FeaturePostRp()
                {
                     Name = item,
                     Description = item,
                     ProductId = product.Id
                });

                var service = await comQueryService.GetServiceByName(product.Id, item);
                var feature = await comQueryFeature.GetFeatureByName(product.Id, item);
                await comServiceMap.CreateServiceMap(new Models.ServiceMapPostRp()
                {
                    FeatureId = feature.Id,
                    ServiceId = service.Id
                });

                await comSource.Create(new Models.SourcePostRp()
                {
                    Name = item + "source",
                    ProductId = product.Id
                });

                var source = await comSource.GetByName(product.Id, item + "source");

                await comIndicators.Create(new Models.IndicatorPostRp()
                {
                    SourceId = source.Id,
                    FeatureId = feature.Id
                });
            }
        }
    }
}

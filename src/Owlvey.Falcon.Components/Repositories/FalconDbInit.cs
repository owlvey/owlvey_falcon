using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Repositories
{
    public static class FalconDbInit
    {

        public static void Migrate(this FalconDbContext dbContext, string env)
        {
            dbContext.Database.Migrate();
            SeedData(dbContext, env);
        }

        private static void SeedData(FalconDbContext dbContext, string env) {

            if (env == "Development" ) {
                var userCreated = "test-user";
                var date = DateTime.UtcNow;
                // Create customer
                var customer = CustomerEntity.Factory.Create(userCreated, date, $"Customer {DateTime.UtcNow.ToShortTimeString()}");
                // Create Squad
                var squad = SquadEntity.Factory.Create("team owlvey", date, userCreated, customer);
                // Create Members
                var users = new[] { "gregory", "ricardo", "felipe", "gustavo" };

                squad.Users = new List<UserEntity>();
                foreach (var item in users)
                {
                    var user = UserEntity.Factory.Create(userCreated, date, item);
                    var member = MemberEntity.Factory.Create(squad, user, date, userCreated);

                    squad.Users.Add(user);
                }

                // Create Product
                var product = ProductEntity.Factory.Create("Application", date, userCreated, customer);

                var biz_names = new[] { "Login", "Onboarding", "Password Recovery",
                "Mesa de cambio", "Mi Lista", "Pago de Servicios", "Pago de Tarjeta de Credito",
                "Pago Prestamo", "Gates", "Perfil", "Limites", "Clave Digital", "Puntos", "Configuracion de tarjetas",
                "Transferencias", "Retiro sin tarjeta", "Dashboard", "Recargas", "Personal Financial Managment",
                "Detalle Producto" };

                foreach (var item in biz_names)
                {
                    var service = ServiceEntity.Factory.Create(item, 99, date, userCreated, product);
                    product.AddService(service);
                    var feature = FeatureEntity.Factory.Create(item, item, date, userCreated, product);
                    product.AddFeature(feature);
                }

                customer.Products.Add(product);

                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
            }

           
        }
    }
}

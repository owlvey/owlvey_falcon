using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Repositories;
//using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.IoC
{
    public static class RepositoryExtensions
    {
        public static void SetupDataBase(this IServiceCollection providers, IConfiguration configuration, string env)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (env.Equals("development", StringComparison.InvariantCultureIgnoreCase))
            {
                providers.AddDbContext<FalconDbContext>(options =>
                    options.UseSqlite(connectionString)
                );
            }
            else
            {
                providers.AddDbContext<FalconDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                           sqlServerOptionsAction: sqlOptions =>
                           {
                               sqlOptions.EnableRetryOnFailure(
                               maxRetryCount: 3,
                               maxRetryDelay: TimeSpan.FromSeconds(30),
                               errorNumbersToAdd: null);
                           }).AddInterceptors(new OwlveyCommandInterceptor())
                );
            }



            /*
            services.AddDbContext<FalconDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                       sqlServerOptionsAction: sqlOptions =>
                       {
                           sqlOptions.EnableRetryOnFailure(
                           maxRetryCount: 3,
                           maxRetryDelay: TimeSpan.FromSeconds(5),
                           errorNumbersToAdd: null);
                       })
            );
            */
        }

    }
}

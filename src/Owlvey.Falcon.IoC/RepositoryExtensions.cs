using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Repositories;
using Owlvey.Falcon.Data.SQLite.Repositories;
using Owlvey.Falcon.Data.SQLite.Context;
using Microsoft.Extensions.HealthChecks;

namespace Owlvey.Falcon.IoC
{
    public static class RepositoryExtensions
    {
        public static void SetupDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddScoped(x => new FalconDbContextFactory().Create(connectionString));
            services.AddDbContext<FalconDbContext>(options =>
                options.UseSqlite(connectionString)
            );
            
        }
        
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            //// Infra - Data
            services.AddScoped<IAppSettingRepository, AppSettingRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IJournalRepository, JournalRepository>();
            services.AddScoped<ISquadRepository, SquadRepository>();
        }
    }
}

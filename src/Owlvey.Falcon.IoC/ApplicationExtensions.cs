using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Identity;

namespace Owlvey.Falcon.IoC
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ASP.NET HttpContext dependency
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Application
            services.AddTransient<AppSettingQueryComponent>();
            services.AddTransient<AppSettingComponent>();

            services.AddTransient<CustomerQueryComponent>();
            services.AddTransient<CustomerComponent>();

            services.AddTransient<ProductQueryComponent>();
            services.AddTransient<ProductComponent>();

            services.AddTransient<JournalQueryComponent>();
            services.AddTransient<JournalComponent>();

            services.AddTransient<FeatureQueryComponent>();
            services.AddTransient<FeatureComponent>();            

            services.AddTransient<SquadQueryComponent>();
            services.AddTransient<SquadComponent>();

            services.AddTransient<ServiceQueryComponent>();
            services.AddTransient<ServiceComponent>();

            services.AddTransient<UserComponent>();

            // Infra
            services.AddAspNetCoreIndentityService();
        }
    }
}

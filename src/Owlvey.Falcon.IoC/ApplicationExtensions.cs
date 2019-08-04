using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Services;
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
            services.AddTransient<IAppSettingQueryComponent, AppSettingQueryComponent>();
            services.AddTransient<IAppSettingComponent, AppSettingComponent>();

            services.AddTransient<ICustomerQueryComponent, CustomerQueryComponent>();
            services.AddTransient<ICustomerComponent, CustomerComponent>();

            services.AddTransient<IFeatureQueryComponent, FeatureQueryComponent>();
            services.AddTransient<IFeatureComponent, FeatureComponent>();

            services.AddTransient<IProductQueryComponent, ProductQueryComponent>();
            services.AddTransient<IProductComponent, ProductComponent>();

            services.AddTransient<IServiceQueryComponent, ServiceQueryComponent>();
            services.AddTransient<IServiceComponent, ServiceComponent>();

            services.AddTransient<ISquadQueryComponent, SquadQueryComponent>();
            services.AddTransient<ISquadComponent, SquadComponent>();

            // Infra
            services.AddAspNetCoreIndentityService();
        }
    }
}

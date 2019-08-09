using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Identity;
using Owlvey.Falcon.Gateways;
using AutoMapper;

namespace Owlvey.Falcon.IoC
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ASP.NET HttpContext dependency
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Components
            services.AddTransient<AppSettingQueryComponent>();
            services.AddTransient<AppSettingComponent>();

            services.AddTransient<CustomerQueryComponent>();
            services.AddTransient<CustomerComponent>();

            services.AddTransient<ProductQueryComponent>();
            services.AddTransient<ProductComponent>();


            services.AddTransient<SourceItemComponent>();
            services.AddTransient<SourceComponent>();
            services.AddTransient<IndicatorComponent>();

            services.AddTransient<FeatureQueryComponent>();
            services.AddTransient<FeatureComponent>();            

            services.AddTransient<SquadQueryComponent>();
            services.AddTransient<SquadComponent>();

            services.AddTransient<ServiceQueryComponent>();
            services.AddTransient<ServiceComponent>();

            services.AddTransient<ServiceMapComponent>();

            services.AddTransient<UserComponent>();
            services.AddTransient<UserQueryComponent>();

            services.AddTransient<MemberComponent>();
            services.AddTransient<MemberQueryComponent>();

            // Gateways
            services.AddTransient<IDateTimeGateway, DateTimeGateway>();

            // Infra
            services.AddAspNetCoreIndentityService();

            // Automapper
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                CustomerComponentConfiguration.ConfigureMappers(cfg);
                ProductComponentConfiguration.ConfigureMappers(cfg);
                FeatureComponentConfiguration.ConfigureMappers(cfg);
                ServiceComponentConfiguration.ConfigureMappers(cfg);
                UserComponentConfiguration.ConfigureMappers(cfg);
                SquadComponentConfiguration.ConfigureMappers(cfg);
                MemberComponentConfiguration.ConfigureMappers(cfg);
                SourceComponentConfiguration.ConfigureMappers(cfg);
                SourceItemComponentConfiguration.ConfigureMappers(cfg);
                IndicatorComponentConfiguration.ConfigureMappers(cfg);
            });

            IMapper mapper = mapperCfg.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

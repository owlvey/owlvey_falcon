using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using AutoMapper;
using Owlvey.Falcon.API.Extensions;

namespace Owlvey.Falcon.IoC
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationProviders(this IServiceCollection servs, IConfiguration configuration)
        {
            // ASP.NET HttpContext dependency
            

            // Components


            servs.AddTransient<AppSettingQueryComponent>();
            servs.AddTransient<AppSettingComponent>();

            servs.AddTransient<CustomerQueryComponent>();
            servs.AddTransient<CustomerComponent>();

            servs.AddTransient<ProductQueryComponent>();
            servs.AddTransient<ProductComponent>();


            servs.AddTransient<SourceItemComponent>();
            servs.AddTransient<SourceComponent>();
            servs.AddTransient<IndicatorComponent>();

            servs.AddTransient<FeatureQueryComponent>();
            servs.AddTransient<FeatureComponent>();

            servs.AddTransient<SquadQueryComponent>();
            servs.AddTransient<SquadComponent>();

            servs.AddTransient<JourneyQueryComponent>();
            servs.AddTransient<JourneyComponent>();

            servs.AddTransient<JourneyMapComponent>();

            servs.AddTransient<UserComponent>();
            servs.AddTransient<UserQueryComponent>();

            servs.AddTransient<MemberComponent>();
            servs.AddTransient<MemberQueryComponent>();

            servs.AddTransient<MigrationComponent>();

            servs.AddTransient<IncidentComponent>();

            servs.AddTransient<CacheComponent>();

            servs.AddTransient<ConfigurationComponent>();

            servs.AddTransient<SecurityRiskComponent>();

            servs.AddTransient<ReliabilityRiskComponent>(); 
            // Gateways
            servs.AddTransient<IDateTimeGateway, DateTimeGateway>();

            // Infra
            servs.AddAspNetCoreIndentityProvider();

            // Automapper
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                CustomerComponentConfiguration.ConfigureMappers(cfg);
                ProductComponentConfiguration.ConfigureMappers(cfg);
                FeatureComponentConfiguration.ConfigureMappers(cfg);
                IncidentComponentConfiguration.ConfigureMappers(cfg);
                JourneyComponentConfiguration.ConfigureMappers(cfg);
                UserComponentConfiguration.ConfigureMappers(cfg);
                SquadComponentConfiguration.ConfigureMappers(cfg);
                MemberComponentConfiguration.ConfigureMappers(cfg);
                SourceComponentConfiguration.ConfigureMappers(cfg);
                SourceItemComponentConfiguration.ConfigureMappers(cfg);
                SquadFeatureComponentConfiguration.ConfigureMappers(cfg);
                IndicatorComponentConfiguration.ConfigureMappers(cfg);
                SourceComponent.ConfigureMappers(cfg);
                SecurityRiskComponent.ConfigureMappers(cfg);
                ReliabilityRiskComponent.ConfigureMappers(cfg);
                MigrationComponent.ConfigureMappers(cfg);
            });

            IMapper mapper = mapperCfg.CreateMapper();
            servs.AddSingleton(mapper);
        }
    }
}

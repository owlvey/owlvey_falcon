using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Extensions
{
    public static class SwaggerRegisterExtensions
    {

        public static void AddCustomSwagger(this IServiceCollection providers, IConfiguration configuration, IHostingEnvironment environtment)
        {
            providers.Configure<SwaggerAppOptions>(configuration.GetSection("Swagger"));

            var sp = providers.BuildServiceProvider();
            var swaggerOptions = sp.GetService<IOptions<SwaggerAppOptions>>();
            var authenticationOptions = sp.GetService<IOptions<AuthorityOptions>>();

            providers.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerOptions.Value.Version, new OpenApiInfo
                {
                    Version = swaggerOptions.Value.Version,
                    Title = swaggerOptions.Value.Title,
                    Description = swaggerOptions.Value.Description,
                    TermsOfService = new Uri(swaggerOptions.Value.TermsOfService, UriKind.Relative),
                    //Contact = new Contact { Name = swaggerOptions.Value.ContactName, Email = swaggerOptions.Value.ContactEmail }
                });


                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"{authenticationOptions.Value.Authority}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "api.auth", "Identity Operations" },
                                { "api", "Api Operations" },
                            }
                        }
                    }
                });                

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, @"Service.xml");

                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.DescribeAllEnumsAsStrings();                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                });

            });

        }

    }
}

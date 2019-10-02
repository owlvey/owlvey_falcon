using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Owlvey.Falcon.Options;
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

        public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environtment)
        {
            services.Configure<SwaggerAppOptions>(configuration.GetSection("Swagger"));

            var sp = services.BuildServiceProvider();
            var swaggerOptions = sp.GetService<IOptions<SwaggerAppOptions>>();
            var authenticationOptions = sp.GetService<IOptions<AuthorityOptions>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerOptions.Value.Version, new Info
                {
                    Version = swaggerOptions.Value.Version,
                    Title = swaggerOptions.Value.Title,
                    Description = swaggerOptions.Value.Description,
                    TermsOfService = swaggerOptions.Value.TermsOfService,
                    Contact = new Contact { Name = swaggerOptions.Value.ContactName, Email = swaggerOptions.Value.ContactEmail }
                });

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "application",
                    TokenUrl = $"{authenticationOptions.Value.Authority}/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "api.auth", "Identity Operations" },
                        { "api", "Api Operations" },
                    }
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, @"Service.xml");

                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.DescribeAllEnumsAsStrings();
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new string[] { } }
                });
            });

        }

    }
}

using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Owlvey.Falcon.API.Extensions;
using Owlvey.Falcon.IoC;
using Owlvey.Falcon.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Prometheus;

namespace Owlvey.Falcon.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(environment.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

            if (environment.IsDevelopment())
            {
                //builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
            Environment = environment;
        }
        public static void LogRequestHeaders(IApplicationBuilder app,
         ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("app.headers");
            app.Use(async (context, next) =>
            {
                var builder = new StringBuilder("\n");
                foreach (var header in context.Request.Headers)
                {
                    builder.AppendLine($"{header.Key}:{header.Value}");
                }
                logger.LogWarning(builder.ToString());
                await next.Invoke();
            });
        }
        public IHostingEnvironment Environment { get; private set;  }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {

            IdentityModelEventSource.ShowPII = true;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                //Authorize Filter
                var policy = new AuthorizationPolicyBuilder(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                  .RequireAuthenticatedUser()
                  .RequireRole("admin", "guest", "integration")
                  .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddControllersWithViews().AddJsonOptions(
                options => options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter()
                    ));

            services.AddApiVersioning(options => {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                }                
            );

            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.MultipartBodyLengthLimit = 268435456;
            });

            services.AddCors();
            services.AddAuthority(Configuration, Environment);
            services.AddApplicationServices(Configuration);
            services.SetupDataBase(Configuration, Environment.EnvironmentName);
            services.AddCustomSwagger(Configuration, Environment);


           

        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IOptions<SwaggerAppOptions> swaggerOptions,
            IConfiguration configuration, FalconDbContext dbContext, IDateTimeGateway dateTimeGateway)
        {
            LogRequestHeaders(app, app.ApplicationServices.GetService<ILoggerFactory>());

            app.UseMetricServer();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO enable use ssl
                //app.UseHsts();
                //app.UseHttpsRedirection();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                dbContext.Migrate(Environment.EnvironmentName);
                dbContext.SeedData(Environment.EnvironmentName, dateTimeGateway.GetCurrentDateTime());
            }

           

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                
                c.DocumentTitle = swaggerOptions.Value.Title;
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint($"{swaggerOptions.Value.Endpoint}", swaggerOptions.Value.Title);
                
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
        }
    }
}

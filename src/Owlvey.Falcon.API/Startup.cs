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

        public IHostingEnvironment Environment { get; private set;  }
        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
            });

            services.AddMvc(options =>
            {
                //Authorize Filter
                var policy = new AuthorizationPolicyBuilder(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                  .RequireAuthenticatedUser()
                  .RequireRole("admin", "guest", "integration")
                  .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
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
            IConfiguration configuration, FalconDbContext dbContext)
        {
            
            app.UseStaticFiles();

            if (env.IsDevelopment() || env.IsDocker())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            //TODO
            if (!env.IsDocker())
            {
                dbContext.Migrate(Environment.EnvironmentName);
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = swaggerOptions.Value.Title;
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint($"{swaggerOptions.Value.Endpoint}", swaggerOptions.Value.Title);
            });

            if (!env.IsDocker() && !env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
        }
    }
}

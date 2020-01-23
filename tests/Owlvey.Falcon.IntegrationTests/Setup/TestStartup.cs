using Microsoft.AspNetCore.Builder;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Owlvey.Falcon.API;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.IoC;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using IdentityServer4.AccessTokenValidation;
using GST.Fake.Authentication.JwtBearer.Events;
using System.Security.Claims;
using System.Threading.Tasks;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    /// <summary>
    /// Integration Test Starup
    /// </summary>
    public class TestStartup : Startup
    {

        /// <summary>
        /// Constructor Test
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="env"></param>
        public TestStartup(IConfiguration conf, IHostingEnvironment env) : base(conf, env)
        {
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services">Service Collection</param>
        public override void ConfigureServices(IServiceCollection services)
        {

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
            }).AddApplicationPart(typeof(Startup).Assembly);

            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            }).AddFakeJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddApplicationServices(Configuration);



            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            //var options = new DbContextOptionsBuilder<FalconDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            /*
             */
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<FalconDbContext>(options =>
                options.UseSqlite(connection)
            );
            
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">App</param>
        /// <param name="env">Env</param>
        /// <param name="loggerFactory">Logger</param>
        /// <param name="swaggerOptions">Swagger Option</param>
        /// <param name="configuration">Configuration</param>
        public override void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IOptions<SwaggerAppOptions> swaggerOptions, 
            IConfiguration configuration, FalconDbContext dbContext, IDateTimeGateway dateTimeGateway)
        {
            app.UseMvc();

            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            // Setup Default Data
            app.UseAuthentication();

            var customer = new Core.Entities.CustomerEntity
            {
                Id = 9999,
                Name = "Default Customer",
                Avatar = "Default",
                CreatedBy = "test",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = "test",
                ModifiedOn = DateTime.UtcNow,                
                Products = new List<Core.Entities.ProductEntity> {
                    new Core.Entities.ProductEntity{
                        Id = 9999,
                        Description= "Default Product",
                        Name = "Default Product",
                        CreatedBy = "test",
                        Avatar="default",
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = "test",
                        ModifiedOn = DateTime.UtcNow,
                        Features = new List<Core.Entities.FeatureEntity> {
                            new FeatureEntity{
                                Id = 9999,
                                Description= "Default Feature",
                                Name = "Default Feature",
                                Avatar = "default",
                                CreatedBy = "test",
                                CreatedOn = DateTime.UtcNow,
                                ModifiedBy = "test",
                                ModifiedOn = DateTime.UtcNow,
                            }
                        },
                        Services = new List<Core.Entities.ServiceEntity>{
                            new Core.Entities.ServiceEntity{
                                 Id = 9999,
                                Description= "Default Service",
                                Name = "Default Service",                                
                                Slo = 99,
                                Avatar = "default",
                                CreatedBy = "test",
                                CreatedOn = DateTime.UtcNow,
                                ModifiedBy = "test",
                                ModifiedOn = DateTime.UtcNow,
                                Group = "Default"
                            }
                        }
                    }
                }
            };
            customer.Squads = new List<SquadEntity> {
                SquadEntity.Factory.Create("Default Squad", dateTimeGateway.GetCurrentDateTime(), "user", customer)
            };
            
            dbContext.Customers.Add(customer);

            var user = UserEntity.Factory.Create("test", dateTimeGateway.GetCurrentDateTime(), "user@falcon.com");
            user.Id = 9999;
            dbContext.Users.Add(user);
            
            dbContext.SaveChanges();

        }


    }

}

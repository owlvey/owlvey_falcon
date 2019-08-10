using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Owlvey.Falcon.API;
using Owlvey.Falcon.API.Controllers;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.IoC;
using Owlvey.Falcon.Options;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

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
            services.AddMvc().AddApplicationPart(typeof(BaseController).Assembly);

            services.AddApplicationServices(Configuration);
            
            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

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
            IConfiguration configuration, FalconDbContext dbContext)
        {
            app.UseMvc();

            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            // Setup Default Data

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
                                SLO = 99,
                                Avatar = "default",
                                CreatedBy = "test",
                                CreatedOn = DateTime.UtcNow,
                                ModifiedBy = "test",
                                ModifiedOn = DateTime.UtcNow,
                            }
                        }
                    }
                }
            };
            customer.Squads = new List<Core.Entities.SquadEntity> {
                SquadEntity.Factory.Create("Default Squad", DateTime.UtcNow, "user", customer)};
            
            dbContext.Customers.Add(customer);

            dbContext.Users.Add(new Core.Entities.UserEntity
            {
                Id = 9999,
                Email = "user@falcon.com",
                CreatedBy = "test",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = "test",
                ModifiedOn = DateTime.UtcNow
            });
            
            dbContext.SaveChanges();

        }


    }

}

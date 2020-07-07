using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Owlvey.Falcon.Repositories
{

    public class FalconDbContextFactory : IDesignTimeDbContextFactory<FalconDbContext>
    {
        public FalconDbContext Create(string connectionString = null)
        {
            return CreateDbContext(new string[] { connectionString });
        }
        
        public FalconDbContext CreateDbContext(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development";

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("dbsettings.json")
            .AddJsonFile($"dbsettings.{env}.json", true)
            .AddEnvironmentVariables()
            .Build();

            var builder = new DbContextOptionsBuilder<FalconDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (String.IsNullOrWhiteSpace(connectionString) == true)
            {
                throw new InvalidOperationException("Could not find a connection string named 'DefaultConnection'.");
            }

            if (args.Length > 0)
            {
                connectionString = args[0];
            }

            if (env.Equals("development", StringComparison.InvariantCultureIgnoreCase))
            {
                builder.UseSqlite(connectionString);
            }
            else
            {
                builder.UseSqlServer(connectionString).AddInterceptors(new OwlveyCommandInterceptor())
                    ;
            }
            
            return new FalconDbContext(builder.Options);
        }
    }
}

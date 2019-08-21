﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("dbsettings.json")
            .AddJsonFile($"dbsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
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

            builder.UseSqlServer(connectionString);
            
            return new FalconDbContext(builder.Options);
        }
    }
}

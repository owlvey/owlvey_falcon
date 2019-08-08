using Microsoft.EntityFrameworkCore;
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
        public FalconDbContext Create(string connectionString = "FalconDb.db")
        {
            return CreateDbContext(new string[] { connectionString });
        }

        public FalconDbContext CreateDbContext(string[] args)
        {
            
            var builder = new DbContextOptionsBuilder<FalconDbContext>();

            var connectionString = "FalconDb.db";

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find a connection string named 'DefaultConnection'.");
            }

            //if (args.Length > 0)
            //{
            //    connectionString = args[0];
            //}
            
            builder.UseSqlite(connectionString);

            return new FalconDbContext(builder.Options);
        }
    }
}

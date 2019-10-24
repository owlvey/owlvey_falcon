﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Repositories
{
    public static class FalconDbInit
    {

        public static void Migrate(this FalconDbContext dbContext, string env)
        {

            System.Console.WriteLine("environment "  + env);     

            dbContext.Database.Migrate();
        }

        public static void SeedData(this FalconDbContext dbContext, string env) {

            if (env == "Development" || env.Equals("docker", StringComparison.InvariantCultureIgnoreCase))
            {
                var userCreated = "test-user";
                var date = DateTime.UtcNow;

                string customerName = $"Awesome Organization";

                // Create Customer
                if (!dbContext.Customers.AnyAsync(x=> x.Name.Equals(customerName)).GetAwaiter().GetResult())
                {
                    var customer = CustomerEntity.Factory.Create(userCreated, date, customerName);

                    dbContext.Customers.Add(customer);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Hosting;
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
            dbContext.Database.Migrate();
            SeedData(dbContext, env);
        }

        private static void SeedData(FalconDbContext dbContext, string env) {

            if (env == "Development" ) {
                
            }
            
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Owlvey.Falcon.Repositories;

namespace Owlvey.Falcon.IoC
{
    public static class RepositoryExtensions
    {
        public static void SetupDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<FalconDbContext>(options =>
                options.UseSqlite(connectionString)
            );
            
        }
        
    }
}

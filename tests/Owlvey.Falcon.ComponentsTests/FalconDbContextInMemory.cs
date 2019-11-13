using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Repositories;

namespace Owlvey.Falcon.ComponentsTests
{
    public class FalconDbContextInMemory : FalconDbContext
    {
        public static DbContextOptions<FalconDbContext> Build() {
            //var options = new DbContextOptionsBuilder<FalconDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            //return options;
            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            var options = new DbContextOptionsBuilder<FalconDbContext>().UseSqlite(connection).Options;
            return options;
        }
        public FalconDbContextInMemory(): base(FalconDbContextInMemory.Build())
        {            
                        
        }
    }
}

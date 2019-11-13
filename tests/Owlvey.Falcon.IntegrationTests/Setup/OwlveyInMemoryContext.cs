using System;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Repositories;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    public class FalconDbContextInMemory : FalconDbContext
    {
        public static DbContextOptions<FalconDbContext> Build()
        {
            var options = new DbContextOptionsBuilder<FalconDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            return options;
        }
        public FalconDbContextInMemory() : base(FalconDbContextInMemory.Build())
        {

        }
    }
}

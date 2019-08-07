using System;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Repositories;

namespace Owlvey.Falcon.ManualTests
{
    public class FalconDbContextInMemory : FalconDbContext
    {
        public static DbContextOptions<FalconDbContext> Build() {
            var options = new DbContextOptionsBuilder<FalconDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            return options;
        }
        public FalconDbContextInMemory(): base(FalconDbContextInMemory.Build())
        {
                        
        }
    }
}

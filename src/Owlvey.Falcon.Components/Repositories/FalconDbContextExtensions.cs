using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Repositories
{
    public static class FalconDbContextExtensions
    {
        public static async Task<int> SaveChangesAsyncRetry<E>(this FalconDbContext context, int maxRetryAttempts = 3, int pauseBetweenFailures = 2) where E : Exception
        {
            var retryPolicy = Policy
                .Handle<E>()
                .WaitAndRetryAsync(maxRetryAttempts, i => TimeSpan.FromSeconds(pauseBetweenFailures));
            return await retryPolicy.ExecuteAsync(async ()=> await context.SaveChangesAsync());
        }
    }
}

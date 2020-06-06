using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Repositories.Sources
{
    public static class SourceExtensions
    {
        public static async Task<SourceEntity> GetSource(this FalconDbContext context,
            int productId, string name)
        {
            return await context.Sources.SingleOrDefaultAsync(c => c.ProductId == productId && c.Name == name);
        }

        public static async Task<SourceEntity> GetSourceWithItems(this FalconDbContext context,
            int sourceId, DatePeriodValue period)
        {
            var source = await context.Sources.SingleOrDefaultAsync(c => c.Id == sourceId);
            source.SourceItems = await context.GetSourceItems(sourceId, period.Start, period.End);
            return source;
        }

    }
}

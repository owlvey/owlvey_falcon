using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.Repositories.Sources
{
    public static class SourceExtensions
    {
        public static async Task<SourceEntity> GetSource(this FalconDbContext context,
            int productId, string name)
        {
            return await context.Sources.SingleOrDefaultAsync(c => c.ProductId == productId && c.Name == name);
        }
    }
}

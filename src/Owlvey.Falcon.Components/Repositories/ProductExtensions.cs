using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Owlvey.Falcon.Repositories.Products
{
    public static class ProductExtensions
    {
        public static async Task<ProductEntity> GetProduct(this FalconDbContext context,
            int customerId, string name)
        {
            return await context.Products.SingleOrDefaultAsync(c => c.CustomerId == customerId && c.Name == name);
        }
    }
}

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

        public static async Task<ProductEntity> FullLoadProduct(this FalconDbContext context,
            int productId)
        {
            var product = await context.Products.Include(c => c.Customer)            
            .Where(c => c.Id == productId).SingleAsync();

            product.Services = await context.Services
                .Include(c => c.FeatureMap)
                .Where(c => c.ProductId == productId).ToListAsync();

            product.Features = await context.Features
                .Include(c => c.Indicators)
                .Where(c => c.ProductId == productId).ToListAsync();

            foreach (var item in product.Services)
            {
                foreach (var map in item.FeatureMap)
                {
                    map.Feature = product.Features.Single(c => c.Id == map.Id);
                }
            }

            var sources = await context.Sources.Where(c => c.ProductId == productId).ToListAsync();

            product.Sources = sources;

            foreach (var feature in product.Features)
            {
                foreach (var sli in feature.Indicators)
                {
                    sli.Source = sources.Where(c => c.Id == sli.SourceId).Single();
                }                   
            }
            return product;
        }
    }
}

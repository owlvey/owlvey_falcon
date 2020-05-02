using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities.Source;

namespace Owlvey.Falcon.Repositories.Products
{
    public static class ProductExtensions
    {
        public static async Task<ProductEntity> GetProduct(this FalconDbContext context,
            int customerId, string name)
        {
            return await context.Products.SingleOrDefaultAsync(c => c.CustomerId == customerId && c.Name == name);
        }

        public static async Task<ProductEntity> FullLoadProductWithGroupAndSourceItems(this FalconDbContext context, int productId, string group, DateTime start, DateTime end)
        {
            var product = await FullLoadProduct(context, productId);
            product.Services = product.Services.Where(c => c.Group == group).ToList();
            var sources = product.Services.SelectMany(c => c.FeatureMap).SelectMany(c => c.Feature.Indicators).Select(c => c.SourceId).Distinct().ToList();
            var sourceItems = await context.GetSourceItems(sources,  start, end);
            foreach (var service in product.Services)
            {
                foreach (var map in service.FeatureMap)
                {
                    foreach (var indicator in map.Feature.Indicators)
                    {
                        indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                    }
                }
            }
            return product;
        }

        

        public static async Task<ProductEntity> FullLoadProductWithSourceItems(this FalconDbContext context, int productId, DateTime start, DateTime end) {
            var product = await FullLoadProduct(context, productId);
            var sourceItems = await context.GetSourceItemsByProduct(productId, start, end);
            foreach (var service in product.Services)
            {
                foreach (var map in service.FeatureMap)
                {
                    foreach (var indicator in map.Feature.Indicators)
                    {
                        indicator.Source.SourceItems = sourceItems.Where(c => c.SourceId == indicator.SourceId).ToList();
                    }
                }
            }
            return product;
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
                    map.Feature = product.Features.Single(c => c.Id == map.FeatureId);
                }
            }

            var sources = await context.Sources.Where(c => c.ProductId == productId).ToListAsync();

            product.Sources = new SourceCollection(sources);

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

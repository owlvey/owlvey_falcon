using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class CacheComponent
    {
        public const string ServiceComponentGetServicesAvailability = "ServiceComponentGetServicesAvailability";
        public const string FeatureComponentGetFeaturesAvailability = "FeatureComponentGetFeaturesAvailability";
        public const string OWLVEY_LAST_MODIFIED = "owlvey_last_modified";
        private static IMemoryCache _cache = new MemoryCache( new MemoryCacheOptions() { });

        private static Dictionary<string, List<string>> Dependencies = new Dictionary<string, List<string>>() {
            {
                CacheComponent.ServiceComponentGetServicesAvailability , new List<string>()
            }
        };

        private FalconDbContext DbContext; 

        public CacheComponent(FalconDbContext dbContext)
        {
            this.DbContext = dbContext; 
        }        

        public async Task<string> GetLastModified() {

            var customer = await this.DbContext.Customers.OrderByDescending(c => c.ModifiedOn ).FirstOrDefaultAsync();
            var product = await this.DbContext.Products.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var service = await this.DbContext.Services.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync(); 
            var serviceMap = await this.DbContext.ServiceMaps.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var feature = await this.DbContext.Features.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var indicator = await this.DbContext.Indicators.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var source = await this.DbContext.Sources.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var sourceItem = await this.DbContext.SourcesItems.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var squad = await this.DbContext.Squads.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();
            var squadFeature = await this.DbContext.SquadFeatures.OrderByDescending(c => c.ModifiedOn).FirstOrDefaultAsync();

            var dates = new List<DateTime?>();            
            dates.Add(customer?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(product?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(service?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(serviceMap?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(feature?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(indicator?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(source?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(sourceItem?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(squad?.ModifiedOn ?? DateTime.MinValue);
            dates.Add(squadFeature?.ModifiedOn ?? DateTime.MinValue);

            return dates.Max().GetValueOrDefault().ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

    }
}

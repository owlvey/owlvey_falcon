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
            string result = "nodata";
            var setting = await this.DbContext.AppSettings.Where(c => c.Key == AppSettingEntity.AppLastModifiedVersion).SingleOrDefaultAsync();
            if (setting != null) {
                result = setting.Value;
            }
            return result;            
        }

    }
}

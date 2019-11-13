using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Owlvey.Falcon.Models;

namespace Owlvey.Falcon.Components
{
    public class CacheComponentA
    {
        public const string ServiceComponentGetServicesAvailability = "ServiceComponentGetServicesAvailability";
        public const string FeatureComponentGetFeaturesAvailability = "FeatureComponentGetFeaturesAvailability";
        private IMemoryCache _cache;
        private static Dictionary<string, List<string>> Dependencies = new Dictionary<string, List<string>>() {
            { CacheComponentA.ServiceComponentGetServicesAvailability , new List<string>() }
        };

        public CacheComponentA()
        {
            this._cache = new MemoryCache(new MemoryCacheOptions() {
                 
            });
        }
        public void Invalidate(int productId, string key)
        {
            if (Dependencies.ContainsKey(key))
            {
                var dependencies = Dependencies[key];
                this._cache.Remove(this.GenerateProductScope(productId, key));
                foreach (var item in dependencies)
                {
                    this._cache.Remove(this.GenerateProductScope(productId, item));
                }
            }
        }

        private string GenerateProductScope(int productId, string key)
        {
            return string.Format("{0}_{1}", productId, key);
        }

        #region Services
        public IEnumerable<ServiceGetListRp> GetServicesAvailability(int productId)
        {
            string name = this.GenerateProductScope(productId, CacheComponentA.ServiceComponentGetServicesAvailability);
            return this._cache.Get<IEnumerable<ServiceGetListRp>>(name);
        }
        public void SetServicesAvailability(int productId, IEnumerable<ServiceGetListRp> value)
        {
            this.Invalidate(productId, ServiceComponentGetServicesAvailability);
            string name = this.GenerateProductScope(productId, CacheComponentA.ServiceComponentGetServicesAvailability);
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            this._cache.Set(name, value, options);
        }

        public void InvalidateServicesCache(int productId) {
            string name = this.GenerateProductScope(productId, CacheComponentA.ServiceComponentGetServicesAvailability);
            this.Invalidate(productId, name);
        }

        #endregion

        #region Features
        public IEnumerable<ServiceGetListRp> GetFeaturesAvailability(int productId)
        {
            string name = this.GenerateProductScope(productId, CacheComponentA.FeatureComponentGetFeaturesAvailability);
            return this._cache.Get<IEnumerable<ServiceGetListRp>>(name);
        }
        public void SetFeaturesAvailability(int productId, IEnumerable<ServiceGetListRp> value)
        {
            this.Invalidate(productId, ServiceComponentGetServicesAvailability);
            string name = this.GenerateProductScope(productId, CacheComponentA.FeatureComponentGetFeaturesAvailability);
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            this._cache.Set(name, value, options);
        }

        public void InvalidateFeaturesCache(int productId)
        {
            string name = this.GenerateProductScope(productId, CacheComponentA.FeatureComponentGetFeaturesAvailability);
            this.Invalidate(productId, name);
        }

       
        #endregion

    }
}

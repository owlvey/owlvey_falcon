using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class ServiceMapModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Service { get; set; }
        public string Feature { get; set; }

        public void Load(string organization, string product, string service, string feature )
        {
            this.Organization = organization;
            this.Product = product;
            this.Service = service;
            this.Feature = feature;            
        }
        public static IEnumerable<ServiceMapModel> Load(string organization, string product, string service, IEnumerable<ServiceMapEntity> entities)
        {
            var result = new List<ServiceMapModel>();
            foreach (var item in entities)
            {
                var model = new ServiceMapModel();
                model.Load(organization, product, service, item.Feature.Name);
                result.Add(model);
            }
            return result;
        }
    }
}

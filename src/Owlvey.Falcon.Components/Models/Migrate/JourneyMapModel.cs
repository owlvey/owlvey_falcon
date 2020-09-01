using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class JourneyMapModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Journey { get; set; }
        public string Feature { get; set; }

        public void Load(string organization, string product, string journey, string feature )
        {
            this.Organization = organization;
            this.Product = product;
            this.Journey = journey;
            this.Feature = feature;            
        }
        public static IEnumerable<JourneyMapModel> Load(string organization,
            string product, string journey, IEnumerable<JourneyMapEntity> entities)
        {
            var result = new List<JourneyMapModel>();
            foreach (var item in entities)
            {
                var model = new JourneyMapModel();
                model.Load(organization, product, journey, item.Feature.Name);
                result.Add(model);
            }
            return result;
        }
    }
}

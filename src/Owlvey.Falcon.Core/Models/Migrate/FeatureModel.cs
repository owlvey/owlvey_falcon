using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class FeatureModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }


        public void Load(string organization, string product, FeatureEntity feature)
        {
            this.Organization = organization;
            this.Product = product;
            this.Name = feature.Name;
            this.Avatar = feature.Avatar;
            this.Description = feature.Description;
        }
        public static IEnumerable<FeatureModel> Load(string organization, string product, IEnumerable<FeatureEntity> entities)
        {
            var result = new List<FeatureModel>();
            foreach (var item in entities)
            {
                var model = new FeatureModel();
                model.Load(organization, product, item);
                result.Add(model);
            }
            return result;
        }

    }
}

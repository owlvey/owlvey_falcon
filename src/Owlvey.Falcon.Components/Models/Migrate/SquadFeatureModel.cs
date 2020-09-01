using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;


namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SquadFeatureModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Feature { get; set; }
        public string Squad { get; set; }

        public void Load(string organization, string product, string feature, string squad)
        {
            this.Organization = organization;
            this.Product = product;
            this.Feature = feature;
            this.Squad = squad;
        }
        public static IEnumerable<SquadFeatureModel> Load(string organization, string product, string feature,
            IEnumerable<SquadFeatureEntity> entities)
        {
            var result = new List<SquadFeatureModel>();
            foreach (var item in entities)
            {
                var model = new SquadFeatureModel();
                model.Load(organization, product, feature, item.Squad.Name);
                result.Add(model);
            }
            return result;
        }

    }
}

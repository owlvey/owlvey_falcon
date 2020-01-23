using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class IndicatorModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Feature { get; set; }
        public string Source { get; set; }

        public void Load(string organization, string product, string feature, string source)
        {
            this.Organization = organization;
            this.Product = product;
            this.Feature = feature;
            this.Source = source;
            
        }
        public static IEnumerable<IndicatorModel> Load(string organization, string product, string feature, IEnumerable<SourceEntity> entities)
        {
            var result = new List<IndicatorModel>();
            foreach (var item in entities)
            {
                var model = new IndicatorModel();
                model.Load(organization, product, feature, item.Name);
                result.Add(model);
            }
            return result;
        }
    }
}

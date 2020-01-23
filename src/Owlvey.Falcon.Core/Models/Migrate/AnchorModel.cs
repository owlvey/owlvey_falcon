using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class AnchorModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }

        public void Load(string organization, string product, AnchorEntity anchor)
        {
            this.Organization = organization;
            this.Product = product;
            this.Name = anchor.Name;
            this.Target = anchor.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture);         
        }
        public static IEnumerable<AnchorModel> Load(string organization, string product, IEnumerable<AnchorEntity> entities)
        {
            var result = new List<AnchorModel>();
            foreach (var item in entities)
            {
                var model = new AnchorModel();
                model.Load(organization, product, item);
                result.Add(model);
            }
            return result;
        }
    }
}

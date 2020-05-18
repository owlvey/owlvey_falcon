using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;


namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceItemModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Source { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }        
        public string Target { get; set; }        

        public decimal Proportion { get; set; }

        public void Load(string organization, string product, string source, SourceItemEntity entity)
        {
            this.Organization = organization;
            this.Product = product;
            this.Source = source;            
            this.Good = entity.Good.GetValueOrDefault();
            this.Total = entity.Total.GetValueOrDefault();            
            this.Target = entity.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            this.Proportion = entity.Measure;
        }
        public static IEnumerable<SourceItemModel> Load(string organization, string product, string source, IEnumerable<SourceItemEntity> entities)
        {
            var result = new List<SourceItemModel>();
            foreach (var item in entities)
            {
                var model = new SourceItemModel();
                model.Load(organization, product, source, item);
                result.Add(model);
            }
            return result;
        }
    }
}

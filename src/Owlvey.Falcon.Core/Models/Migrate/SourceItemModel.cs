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
        public string Start { get; set; }
        public string End { get; set; }

        public void Load(string organization, string product, string source, SourceItemEntity entity)
        {
            this.Organization = organization;
            this.Product = product;
            this.Source = source;
            this.Good = entity.Good;
            this.Total = entity.Total;
            this.End = entity.End.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            this.Start = entity.Start.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
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

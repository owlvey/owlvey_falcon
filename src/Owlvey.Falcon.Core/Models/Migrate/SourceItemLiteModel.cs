using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceItemLiteModel
    {        
        public string Source { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public string Target { get; set; }

        public void Load(string source, SourceItemEntity entity)
        {        
            this.Source = source;
            this.Good = entity.Good;
            this.Total = entity.Total;
            this.Target = entity.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static IEnumerable<SourceItemLiteModel> Loads(string source, IEnumerable<SourceItemEntity> entities)
        {
            var result = new List<SourceItemLiteModel>();
            foreach (var item in entities)
            {
                var model = new SourceItemLiteModel();
                model.Load(source, item);
                result.Add(model);
            }
            return result;
        }
    }
}

using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceItemLiteModel
    {        
        public string Source { get; set; }
        public string Group {get ; set;}
        public int Good { get; set; }
        public int Total { get; set; }
        public string Target { get; set; }
        public decimal Measure { get; set; }
        
        public static IEnumerable<SourceItemEntity> Build(ProductEntity product, 
            DateTime on, string CreatedBy, ISheet sheet){
            var result = new List<SourceItemEntity>();
            for (int row = 2; row <= sheet.getRows(); row++)
            {                   
                var source = sheet.get<string>(row, 1);
                var group = sheet.get<string>(row, 2);
                var good = sheet.get<int>(row, 3);
                var total = sheet.get<int>(row, 4);                    
                var target = DateTime.Parse(sheet.get<string>(row, 5));
                var measure = sheet.get<decimal>(row, 6);                                    
                var sourceInstance = product.Sources.Where(e=>e.Name == source).Single();                               
                var targetGroup = sourceInstance.ParseGroup(group);
                var item = SourceEntity.Factory.CreateItem(sourceInstance, target, good, total, on,
                     CreatedBy, targetGroup);                                                    
                result.Add(item);
            }            
            return result;
        }
        public void Load(string source, SourceItemEntity entity)
        {        
            this.Source = source;            
            this.Good = entity.Good.GetValueOrDefault();
            this.Total = entity.Total.GetValueOrDefault();
            
            this.Measure = entity.Measure;
            this.Target = entity.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            this.Group = entity.Group.ToString();
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

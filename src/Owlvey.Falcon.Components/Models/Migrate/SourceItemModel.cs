using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;


namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceItemModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Source { get; set; }
        public string Group { get; set;}
        public int Good { get; set; }
        public int Total { get; set; }        
        public string Target { get; set; }        
        public decimal Measure { get; set; }


        public static IEnumerable<SourceItemEntity> Build(IEnumerable<CustomerEntity> customers, 
            DateTime on, string CreatedBy, ISheet sheet){
            var result = new List<SourceItemEntity>();
            for (int row = 2; row <= sheet.getRows(); row++)
            {   
                var organization = sheet.get<string>(row, 1);
                var product = sheet.get<string>(row, 2);
                var source = sheet.get<string>(row, 3);
                var group = sheet.get<string>(row, 4);
                var good = sheet.get<int>(row, 5);
                var total = sheet.get<int>(row, 6);                    
                var target = DateTime.Parse(sheet.get<string>(row, 7));
                var measure = sheet.get<decimal>(row, 8);                    

                var sourceInstance = customers.Where(c=>c.Name == organization).Single()
                    .Products.Where(e=>e.Name == product).Single().Sources.Where(e=>e.Name == source).Single();
               
                var targetGroup = sourceInstance.ParseGroup(group);
                var item = SourceEntity.Factory.CreateItem(sourceInstance, target, good, total, on, CreatedBy, targetGroup);                                                    
                result.Add(item);
            }            
            return result;
        }
        public void Load(string organization, string product, string source, SourceItemEntity entity)
        {
            this.Organization = organization;
            this.Product = product;
            this.Source = source;            
            this.Group = entity.Group.ToString();
            this.Good = entity.Good.GetValueOrDefault();
            this.Total = entity.Total.GetValueOrDefault();            
            this.Target = entity.Target.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            this.Measure = entity.Measure;
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

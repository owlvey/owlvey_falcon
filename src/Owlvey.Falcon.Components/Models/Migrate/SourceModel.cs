using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceModel
    {
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }        
        public string GoodDefinitionAvailability { get; set; }
        public string TotalDefinitionAvailability { get; set; }

        public string GoodDefinitionLatency { get; set; }
        public string TotalDefinitionLatency { get; set; }

        public string GoodDefinitionExperience { get; set; }
        public string TotalDefinitionExperience { get; set; }        
        public decimal Percentile { get; set; }        
        public string Tags {get; set;}
        
        public static IEnumerable<SourceEntity> Build(IEnumerable<CustomerEntity> customers,
            DateTime on, string CreatedBy, ISheet sheet){
            var result = new List<SourceEntity>();
            for (int row = 2; row <= sheet.getRows(); row++)
            {   
                var organization = sheet.get<string>(row, 1);
                var product = sheet.get<string>(row, 2);
                var name = sheet.get<string>(row, 3);
                var avatar = sheet.get<string>(row, 4);
                var description = sheet.get<string>(row, 5);                
                var ava_good = sheet.get<string>(row, 6);                
                var ava_total = sheet.get<string>(row, 7);               
                var lat_good = sheet.get<string>(row, 8);                
                var lat_total = sheet.get<string>(row, 9);                
                var exp_good = sheet.get<string>(row, 10);                
                var exp_total = sheet.get<string>(row, 11);                                 
                var percentile = sheet.get<decimal>(row, 12);               
                var tags = sheet.get<string>(row, 13);               
                
                var productInstance = customers.Where(c => c.Name == organization).Single()
                    .Products.Where(c=> c.Name == product).Single(); 
                var entity = SourceEntity.Factory.Create(productInstance, name, on,  CreatedBy);
                entity.Update(name, avatar, 
                    new Values.DefinitionValue(ava_good, ava_total),
                    new Values.DefinitionValue(lat_good, lat_total),
                    new Values.DefinitionValue(exp_good, exp_total),
                    on, CreatedBy, tags, description, percentile); 

                result.Add(entity);
            }            
            return result;
        }

        public void Load(string organization, string product, SourceEntity entity)
        {
            this.Organization = organization;
            this.Product = product;
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description;            
            this.GoodDefinitionAvailability = entity.GoodDefinitionAvailability;
            this.TotalDefinitionAvailability = entity.TotalDefinitionAvailability;

            this.GoodDefinitionLatency = entity.GoodDefinitionLatency;
            this.TotalDefinitionLatency = entity.TotalDefinitionLatency;

            this.GoodDefinitionExperience = entity.GoodDefinitionExperience;
            this.TotalDefinitionExperience = entity.TotalDefinitionExperience;

            this.Percentile = entity.Percentile;
            this.Tags = entity.Tags;
        }
        public static IEnumerable<SourceModel> Load(string organization, string product, IEnumerable<SourceEntity> entities)
        {
            var result = new List<SourceModel>();
            foreach (var item in entities)
            {
                var model = new SourceModel();
                model.Load(organization, product, item);
                result.Add(model);
            }
            return result;
        }
    }
}

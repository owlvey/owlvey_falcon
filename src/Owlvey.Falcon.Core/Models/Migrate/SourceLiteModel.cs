using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SourceLiteModel
    {
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
        public string Tags { get; set;}
        public static IEnumerable<SourceEntity> Build(ProductEntity product,
            DateTime on, string CreatedBy, ISheet sheet){
            var result = new List<SourceEntity>();
            for (int row = 2; row <= sheet.getRows(); row++)
            {                   
                var name = sheet.get<string>(row, 1);
                var avatar = sheet.get<string>(row, 2);
                var description = sheet.get<string>(row, 3);                
                var ava_good = sheet.get<string>(row, 4);                
                var ava_total = sheet.get<string>(row, 5);               
                var lat_good = sheet.get<string>(row, 6);                
                var lat_total = sheet.get<string>(row, 7);                
                var exp_good = sheet.get<string>(row, 8);                
                var exp_total = sheet.get<string>(row, 9);                                 
                var percentile = sheet.get<decimal>(row, 10);               
                var tags = sheet.get<string>(row, 11);                                               
                var entity = SourceEntity.Factory.Create(product, name, on,  CreatedBy);
                entity.Update(name, avatar, 
                    new Values.DefinitionValue(ava_good, ava_total),
                    new Values.DefinitionValue(lat_good, lat_total),
                    new Values.DefinitionValue(exp_good, exp_total),
                    on, CreatedBy, tags, description, percentile); 

                result.Add(entity);
            }            
            return result;
        }
        public void Load(SourceEntity entity)
        {            
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description;            
            this.GoodDefinitionAvailability = entity.GoodDefinitionAvailability;
            this.TotalDefinitionAvailability = entity.TotalDefinitionAvailability;

            this.GoodDefinitionLatency= entity.GoodDefinitionLatency;
            this.TotalDefinitionLatency = entity.TotalDefinitionLatency;

            this.GoodDefinitionExperience = entity.GoodDefinitionExperience;
            this.TotalDefinitionExperience = entity.TotalDefinitionExperience;

            this.Percentile = entity.Percentile;
            this.Tags = entity.Tags;
        }
        public static IEnumerable<SourceLiteModel> Load(IEnumerable<SourceEntity> entities)
        {
            var result = new List<SourceLiteModel>();
            foreach (var item in entities)
            {
                var model = new SourceLiteModel();
                model.Load(item);
                result.Add(model);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

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

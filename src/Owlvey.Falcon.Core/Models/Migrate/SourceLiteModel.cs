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
        public string Group { get; set; }
        public string GoodDefinitionAvailability { get; set; }
        public string TotalDefinitionAvailability { get; set; }

        public string GoodDefinitionLatency { get; set; }
        public string TotalDefinitionLatency { get; set; }

        public string GoodDefinitionExperience { get; set; }
        public string TotalDefinitionExperience { get; set; }

        public string Kind { get; set; }

        public decimal Percentile { get; set; }

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

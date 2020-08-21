using Owlvey.Falcon.Core.Aggregates;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.ComTypes;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Tags { get; set; }

        [Required]
        public string GoodDefinition { get; set; }

        [Required]
        public string TotalDefinition { get; set; }
        
        public string Avatar { get; set; }

        public string Description { get; set; }                

        public decimal Percentile { get; set; } = 0.95m;

        public virtual ProductEntity Product { get; set; }

        public int ProductId { get; set; }
        
        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();

        public virtual ICollection<SourceItemEntity> SourceItems { get; set; } = new List<SourceItemEntity>();

        public decimal Latency { get; set; }
                
        public virtual void Update(
            string name, string avatar,
            string goodDefinition, string totalDefinition,            
            DateTime on, string modifiedBy, string tags, string description)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Avatar = string.IsNullOrWhiteSpace(avatar) ? this.Avatar : avatar;
            this.GoodDefinition = string.IsNullOrWhiteSpace(goodDefinition) ? this.GoodDefinition : goodDefinition;
            this.TotalDefinition = string.IsNullOrWhiteSpace(totalDefinition) ? this.TotalDefinition : totalDefinition;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.ModifiedOn = on;
            this.Tags = tags ?? this.Tags;
            this.ModifiedBy = modifiedBy;            
        }

        public virtual void Update(
         string name, string avatar,
         string goodDefinition, string totalDefinition,
         DateTime on, string modifiedBy, string tags, string description, decimal percentile)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Avatar = string.IsNullOrWhiteSpace(avatar) ? this.Avatar : avatar;
            this.GoodDefinition = string.IsNullOrWhiteSpace(goodDefinition) ? this.GoodDefinition : goodDefinition;
            this.TotalDefinition = string.IsNullOrWhiteSpace(totalDefinition) ? this.TotalDefinition : totalDefinition;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.ModifiedOn = on;
            this.Percentile = percentile;
            this.Tags = tags ?? this.Tags;
            this.ModifiedBy = modifiedBy;
        }

        public SourceItemEntity AddSourceItem(int good, int total, DateTime target, DateTime on, string createdBy, SourceGroupEnum group)
        {
            
            var result = Factory.CreateItem(this, target, good, total, on, createdBy, group);
            this.SourceItems.Add(result);
            return result;
        }

        public Dictionary<string, int> FeaturesToDictionary() {
            return this.Indicators.ToDictionary(d => d.Feature.Name, c => c.Feature.Id.Value); 
        }

        public SourceGroupEnum ParseGroup(string value) {
            return (SourceGroupEnum)Enum.Parse(typeof(SourceGroupEnum), value);
        }


    }
}

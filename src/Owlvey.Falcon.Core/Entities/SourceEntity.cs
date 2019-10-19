using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
    public class SourceEntityComparer : IEqualityComparer<SourceEntity>
    {
        public bool Equals(SourceEntity x, SourceEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SourceEntity obj)
        {
            return obj.Id.Value;
        }
    }


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

        public virtual ProductEntity Product { get; set; }

        public int ProductId { get; set; }
        
        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();

        public virtual ICollection<SourceItemEntity> SourceItems { get; set; } = new List<SourceItemEntity>();
          
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity: BaseEntity
    {



        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public float SLO { get; set; }

        public string Avatar { get; set; }

        public int ProductId { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual ICollection<FeatureEntity> Features { get; set; }

        public void Update(DateTime on, string modifiedBy, string name, float slo, string description = null, string avatar = null)
        {
            this.Name = name ?? this.Name;
            this.SLO = slo;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = on;
            this.Validate();
        }

    }
}

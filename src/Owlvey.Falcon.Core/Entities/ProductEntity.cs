using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }
        public string Description { get; set; }

        public CustomerEntity Customer { get; set; }
        public int CustomerId { get; set; }

        public virtual ICollection<ServiceEntity> Services { get; set; } = new List<ServiceEntity>();
        public virtual ICollection<FeatureEntity> Features { get; set; } = new List<FeatureEntity>();
        public virtual ICollection<SourceEntity> Sources { get; set; } = new List<SourceEntity>();

        public virtual ICollection<IncidentEntity> Incidents { get; set; } = new List<IncidentEntity>();

        public void AddService(ServiceEntity entity) {
            entity.Product = this;
            this.Services.Add(entity);
        }
        
        public void AddFeature(FeatureEntity entity) {
            entity.Product = this;
            this.Features.Add(entity); 
        }

        public virtual void Update(DateTime on, string modifiedBy, string name, string description, string avatar)
        {
            this.Name = name ?? this.Name;
            this.Description = description ?? this.Description;
            this.Avatar = avatar ?? this.Avatar;
            this.ModifiedOn = on;
            this.ModifiedBy = modifiedBy;
        }
    }
}

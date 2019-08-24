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

        public virtual ICollection<ServiceEntity> Services { get; set; }
        public virtual ICollection<FeatureEntity> Features { get; set; }
        public virtual ICollection<SourceEntity> Sources { get; set; }
        
        public ProductEntity() {
            this.Services = new List<ServiceEntity>();
            this.Features = new List<FeatureEntity>();
            this.Sources = new List<SourceEntity>(); 
        }

        public void AddService(ServiceEntity entity) {
            entity.Product = this;
            this.Services.Add(entity);
        }
        
        public void AddFeature(FeatureEntity entity) {
            entity.Product = this;
            this.Features.Add(entity); 
        }

        public virtual void Update(DateTime on, string modifiedBy)
        {
            this.ModifiedOn = on;
            this.ModifiedBy = modifiedBy;
        }
    }
}

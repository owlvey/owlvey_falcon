using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity : BaseEntity
    {
        public ProductEntity() {
            this.Services = new List<ServiceEntity>();
            this.Features = new List<FeatureEntity>(); 
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public CustomerEntity Customer { get; set; }

        public virtual ICollection<ServiceEntity> Services { get; set; }

        public virtual ICollection<FeatureEntity> Features { get; set; }

        public void AddService(ServiceEntity entity) {
            this.Services.Add(entity);
        }
        public void AddFeature(FeatureEntity entity) {
            this.Features.Add(entity); 
        }
    }
}

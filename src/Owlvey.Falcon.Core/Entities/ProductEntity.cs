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
        public string Description { get; set; }

        public CustomerEntity Customer { get; set; }

        public virtual ICollection<ServiceEntity> Services { get; set; }

        public virtual ICollection<FeatureEntity> Features { get; set; }
    }
}

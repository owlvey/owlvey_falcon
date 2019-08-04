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
        [Required]
        public string Description { get; set; }
        [Required]
        public float SLO { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<FeatureEntity> Features { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class ServiceEntity: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float SLO { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<FeatureEntity> Features { get; set; }
        
    }
}

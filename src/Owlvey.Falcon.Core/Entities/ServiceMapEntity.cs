using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceMapEntity : BaseEntity
    {
        public virtual ServiceEntity Service { get; set; }
        public int ServiceId { get; set; }
        public virtual FeatureEntity Feature { get; set; }
        public int FeatureId { get; set; }
    }
}

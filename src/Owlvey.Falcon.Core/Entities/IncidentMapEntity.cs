using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class IncidentMapEntity : BaseEntity
    {
        public virtual FeatureEntity  Feature { get; set;}
        public int FeatureId { get; set; }
        public virtual IncidentEntity Incident { get; set; }
        public int IncidentId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class JourneyMapEntity : BaseEntity
    {
        public virtual JourneyEntity Journey { get; set; }
        public int JourneyId { get; set; }
        public virtual FeatureEntity Feature { get; set; }
        public int FeatureId { get; set; }
    }
}

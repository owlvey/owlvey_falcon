using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class FeatureEntity: BaseEntity
    {
        public string Name { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<IndicatorEntity> Indicators { get; set; }
    }
}

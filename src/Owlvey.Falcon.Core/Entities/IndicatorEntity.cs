using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Entities
{

    public class IndicatorEntityComparer : IEqualityComparer<IndicatorEntity>
    {
        public bool Equals(IndicatorEntity x, IndicatorEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(IndicatorEntity obj)
        {
            return obj.GetHashCode();
        }
    }

    public partial class IndicatorEntity: BaseEntity
    {
        public int SourceId { get; set; }
        public virtual SourceEntity Source { get; set; }
        public virtual FeatureEntity Feature { get; set; }
        public int FeatureId { get; set; }
        
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

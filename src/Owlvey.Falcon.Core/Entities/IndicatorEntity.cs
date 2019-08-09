using System;
namespace Owlvey.Falcon.Core.Entities
{
    public partial class IndicatorEntity: BaseEntity
    {
        public int SourceId { get; set; }
        public virtual SourceEntity Source { get; set; }
        public int FeatureId { get; set; }
        public virtual FeatureEntity Feature { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

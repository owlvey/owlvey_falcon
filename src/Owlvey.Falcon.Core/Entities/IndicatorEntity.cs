using System;
namespace Owlvey.Falcon.Core.Entities
{
    public partial class IndicatorEntity: BaseEntity
    {
        public virtual SourceEntity Source { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

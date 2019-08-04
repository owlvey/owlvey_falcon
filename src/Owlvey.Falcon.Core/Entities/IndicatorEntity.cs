using System;
namespace Owlvey.Falcon.Core.Entities
{
    public class IndicatorEntity: BaseEntity
    {
        public virtual SourceEntity Journal { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

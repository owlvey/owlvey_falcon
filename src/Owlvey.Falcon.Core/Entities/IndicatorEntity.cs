using System;
namespace Owlvey.Falcon.Core.Entities
{
    public class IndicatorEntity: BaseEntity
    {
        public virtual JournalEntity Journal { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

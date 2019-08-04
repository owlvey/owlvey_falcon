using System;
namespace Owlvey.Falcon.Core.Entities
{
    public class JournalItemEntity: BaseEntity
    {
        public virtual JournalEntity Journal { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
    }
}

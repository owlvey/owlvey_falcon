using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public class JournalItemEntity: BaseEntity
    {
        public virtual JournalEntity Journal { get; set; }
        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }
    }
}

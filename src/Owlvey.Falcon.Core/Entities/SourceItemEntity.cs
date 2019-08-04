using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public class SourceItemEntity: BaseEntity
    {
        public virtual SourceEntity Journal { get; set; }
        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }
    }
}

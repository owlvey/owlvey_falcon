using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity: BaseEntity
    {
        [Required]
        public string GoodDefinition { get; set; }
        [Required]
        public string TotalDefinition { get; set; }
        public string Avatar { get; set; }               

        public virtual ICollection<SourceItemEntity> JournalItems { get; set; }
    }
}

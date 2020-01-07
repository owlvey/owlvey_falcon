using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ClueEntity : BaseEntity
    {
        public SourceItemEntity SourceItem { get; set; } 
        public int SourceItemId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public decimal Value { get; set; } = 0;

        public ClueEntity()
        {

        }

    }
}

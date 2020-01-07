using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public class ClueEntity : BaseEntity
    {
        public SourceEntity Source { get; set; } 
        public int SourceId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int Value { get; set; } = 0;

        public ClueEntity()
        {

        }

    }
}

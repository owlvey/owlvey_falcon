using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
 
    public abstract partial class SourceItemEntity: BaseEntity
    {
        public int SourceId { get; set; }

        public virtual SourceEntity Source { get; set; }
        

        [Required]
        public DateTime Target { get; set; }

        [Required]
        public decimal Proportion { get; set; }

        [Required]
        public SourceKindEnum Kind { get; set; }

        public virtual ICollection<ClueEntity> Clues { get; set; } = new List<ClueEntity>();


        public IDictionary<string, decimal> ExportClues() {
            var result = new Dictionary<string, decimal>();
            foreach (var clue in this.Clues)
            {
                result.Add(clue.Name, clue.Value);    
            }
            return result; 
        }
        public void Update(decimal proportion, DateTime target)
        {
            this.Target = target;
            this.Proportion = proportion;
        }
    }
}

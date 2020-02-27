using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
    public class SourceItemEntityComparer : IEqualityComparer<SourceItemEntity>
    {
        public bool Equals(SourceItemEntity x, SourceItemEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SourceItemEntity obj)
        {
            return obj.Id.Value;
        }
    }
    public partial class SourceItemEntity: BaseEntity
    {
        public int SourceId { get; set; }

        public virtual SourceEntity Source { get; set; }
        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }

        [Required]
        public DateTime Target { get; set; }
        

        public virtual ICollection<ClueEntity> Clues { get; set; } = new List<ClueEntity>();


        public IDictionary<string, decimal> ExportClues() {
            var result = new Dictionary<string, decimal>();
            foreach (var clue in this.Clues)
            {
                result.Add(clue.Name, clue.Value);    
            }
            return result; 
        }

        [NotMapped]
        public decimal Availability { get {
                return QualityUtils.CalculateAvailability(this.Total, this.Good, 1);                
            } }

        public void Update(int total, int good, DateTime target)
        {
            this.Total = total;
            this.Good = good;            
            this.Target = target;
        }
    }
}

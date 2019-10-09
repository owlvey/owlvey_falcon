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
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        [NotMapped]
        public decimal Availability { get {
                return AvailabilityUtils.CalculateAvailability(this.Total, this.Good, 1);                
            } }

        
    

        public void Update(int total, int good, DateTime start, DateTime end)
        {
            this.Total = total;
            this.Good = good;
            this.Start = start;
            this.End = end;
        }


    }
}

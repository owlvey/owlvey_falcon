using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Owlvey.Falcon.Core.Entities
{
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
                if (this.Total > 0)
                    return Decimal.Divide(this.Good, this.Total);
                else {
                    return 1;
                }
            } }

        public class EqualityComparer : IEqualityComparer<SourceItemEntity>
        {
            public bool Equals(SourceItemEntity x, SourceItemEntity y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(SourceItemEntity obj)
            {
                return obj.GetHashCode();
            }
        }

        public SourceItemEntity Clone() {
            SourceItemEntity result = new SourceItemEntity();
            result.CreatedBy = this.CreatedBy;
            result.CreatedOn = this.CreatedOn;
            result.ModifiedBy = this.ModifiedBy;
            result.ModifiedOn = this.ModifiedOn;
            result.Source = this.Source;
            result.SourceId = this.SourceId;
            result.Start = this.Start;
            result.End = this.Start;
            result.Good = this.Good;
            result.Total = this.Total; 
            return result; 
        }


    }
}

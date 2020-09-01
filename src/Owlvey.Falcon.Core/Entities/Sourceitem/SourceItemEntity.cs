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
                        
        public int? Good { get; set; }

        [Required]
        public SourceGroupEnum Group { get; set; }

        [NotMapped]
        public int? Bad { get {
                return this.Total - this.Good;
            } }
        
        public int? Total { get; set; }
    

        [Required]
        public DateTime Target { get; set; }

        [Required]
        public decimal Measure { get; set; }
                
        public void Update(decimal proportion, DateTime target)
        {
            this.Target = target;
            this.Measure = proportion;
        }        
        public void Update(int total, int good, DateTime target)
        {
            this.Total = total;
            this.Good = good;
            this.Measure = QualityUtils.CalculateProportion(total, good);
            this.Target = target;
        }
    }
}

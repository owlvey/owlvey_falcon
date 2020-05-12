using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public class InteractionSourceItemEntity : SourceItemEntity
    {
        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }
        public InteractionSourceItemEntity() {
            this.Kind = SourceKindEnum.Interaction;
        }
        public void Update(int total, int good, DateTime target)
        {
            this.Total = total;
            this.Good = good;
            this.Proportion = QualityUtils.CalculateProportion(total, good);
            this.Target = target;
        }
    }
}

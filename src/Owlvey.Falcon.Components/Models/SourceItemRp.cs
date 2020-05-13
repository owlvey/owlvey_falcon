using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Models
{
    public class SourceItemBaseRp
    {
        public int Id { get; set; }
    }

    public class InteractiveSourceItemGetRp : SourceItemBaseRp
    {
        public InteractiveSourceItemGetRp() { }
        public InteractiveSourceItemGetRp(InteractionSourceItemEntity entity) {
            this.Good = entity.Good;
            this.Total = entity.Total;
            this.Target = entity.Target;
            this.CreatedBy = entity.CreatedBy;
            this.CreatedOn = entity.CreatedOn;
            this.Proportion = entity.Proportion;            
        }
        public int Good { get; set; }
        public int Total { get; set; }
        public DateTime Target { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public decimal Proportion { get; set; }        
    }

    public class ProportionSourceItemGetRp : SourceItemBaseRp
    {        
        public DateTime Target { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public decimal Proportion { get; set; }
        public IDictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();
    }

    public class SourceItemMigrationRp {
        public string Product { get; set; }
        public string Source { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public string Target { get; set; }
        public string Clues { get; set; }
        public decimal Proportion { get; set; }
    }



    public class SourceItemGetListRp : SourceItemBaseRp
    {
        public int SourceId { get; set; }        
        public decimal Proportion { get; set; }
        public DateTime Target { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class InteractionSourceItemGetListRp : SourceItemBaseRp {
        public int SourceId { get; set; }
        public decimal Proportion { get; set; }
        public int Total { get; set; }
        public int Good { get; set; }
        public DateTime Target { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public abstract class SourceItemPostRp{
        [Required]
        public int SourceId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        public IDictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();

        
    }

    public class SourceItemInteractionPostRp: SourceItemPostRp
    {        
        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }        
     
    }


    
    public class SourceItemProportionPostRp : SourceItemPostRp
    {
        [Required]
        public decimal Proportion { get; set; }
    }

    public class SourceItemPutRp
    {
        public string Value { get; set; }
    }
}

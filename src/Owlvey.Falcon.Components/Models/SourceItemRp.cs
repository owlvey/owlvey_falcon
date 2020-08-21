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
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime Target { get; set; }
    }

   

    public class ProportionSourceItemGetRp : SourceItemBaseRp
    {   
        public decimal Measure { get; set; }        
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
        public decimal Measure { get; set; }
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
    }

    public class SourceItemAvailabilityPostRp : SourceItemPostRp
    {

        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }
        [Required]
        public decimal Measure { get; set; }
    }
    public class SourceItemExperiencePostRp : SourceItemPostRp
    {        

        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }
        [Required]
        public decimal Measure { get; set; }
    }

    public class SourceItemLatencyPostRp : SourceItemPostRp
    {        
        [Required]
        public decimal Measure { get; set; }
    }

    public class SourceItemPutRp
    {
        public string Value { get; set; }
    }
}

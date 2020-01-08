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

    public class SourceItemGetRp : SourceItemBaseRp
    {
        public int SourceId { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public decimal Availability { get; set; }

        public IDictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();

    }

    public class SourceItemMigrationRp {
        public string Product { get; set; }
        public string Source { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

        public string Clues { get; set; }
    }

    

    public class SourceItemGetListRp : SourceItemBaseRp
    {
        public int SourceId { get; set; }
        public int Good { get; set; }
        public int Total { get; set; }
        public decimal Availability { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SourceItemPostRp
    {
        [Required]
        public int SourceId { get; set; }        
        [Required]
        public int Good { get; set; }
        [Required]
        public int Total { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        public IDictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();
    }

    public class SourceItemPropotionPostRp
    {
        [Required]
        public int SourceId { get; set; }        
        [Required]
        public decimal Proportion { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        public IDictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();

    }

    public class SourceItemPutRp
    {
        public string Value { get; set; }
    }
}

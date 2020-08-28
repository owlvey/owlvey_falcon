using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Owlvey.Falcon.Models
{
    public class SourceItemBaseRp
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime Target { get; set; }

      
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


    public class SourceItemBatchPostRp {

        [Required]
        public string Customer { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public SourceKindEnum Kind { get; set; } = SourceKindEnum.Interaction;

        [Required]
        public List<string> Items { get; set; } = new List<string>();
        [Required]
        public char Delimiter { get; set; } = ',';
        public class SourceItemBatchResultPostRp {
            public int SourceCreated { get; set; }
            public int ItemsCreated { get; set; }
        }
        public class SourceItemAllInteractionItemPostRp {
            [Required]
            public string Source { get; set; }

            [Required]
            public int Total { get; set; }

            [Required]
            public DateTime Start { get; set; }

            [Required]
            public DateTime End { get; set; }

            [Required]
            public int Availability { get; set; }

            [Required]
            public int Experience { get; set; }

            [Required]
            public decimal Latency { get; set; }
        }

        public List<SourceItemAllInteractionItemPostRp> ParseItems() {
            List<SourceItemAllInteractionItemPostRp> result = new List<SourceItemAllInteractionItemPostRp>();
            foreach (var item in this.Items)
            {
                int delimiterCount = item.Count(c => c == this.Delimiter);
                if (delimiterCount != 6) {
                    throw new ApplicationException($"invalid input  too many delimiter ${delimiterCount } in ${item}");
                }
                var parts = item.Split(this.Delimiter);
                var temp = new SourceItemAllInteractionItemPostRp
                {
                    Source = parts[0],
                    Start = DateTime.Parse(parts[1]),
                    End = DateTime.Parse(parts[2]),
                    Total = int.Parse(parts[3]),
                    Availability = int.Parse(parts[4]),
                    Experience = int.Parse(parts[5]),
                    Latency = decimal.Parse(parts[6])
                };
                result.Add(temp);
            }
            return result;
        }

    }
    public class SourceItemAvailabilityPostRp : SourceItemPostRp
    {        
        public int? Good { get; set; }
        
        public int? Total { get; set; }
        
        public decimal? Measure { get; set; }
    }
    public class SourceItemExperiencePostRp : SourceItemPostRp
    {           
        public int? Good { get; set; }
        
        public int? Total { get; set; }
        
        public decimal? Measure { get; set; }
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

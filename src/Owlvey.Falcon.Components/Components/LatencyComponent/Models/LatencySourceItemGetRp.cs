using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class LatencySourceItemGetRp: SourceItemBaseRp
    {
        public decimal Measure { get; set; }
    }
    public class AvailabilitySourceItemGetRp: SourceItemBaseRp
    {
        public decimal Measure { get; set; }
        public int Total {get;set;}
        public int Good {get;set;}
        public int Delta { get {
                return this.Total - this.Good;
            } 
        }
    }
    public class ExperienceSourceItemGetRp: SourceItemBaseRp
    {
        public decimal Measure { get; set; }
        public int Total {get;set;}
        public int Good {get;set;}
        public int Delta
        {
            get
            {
                return this.Total - this.Good;
            }
        }
    }
}

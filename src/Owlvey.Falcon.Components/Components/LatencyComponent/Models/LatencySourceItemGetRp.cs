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
        public decimal Total {get;set;}
        public decimal Good {get;set;}
    }
    public class ExperienceSourceItemGetRp: SourceItemBaseRp
    {
        public decimal Measure { get; set; }
        public decimal Total {get;set;}
        public decimal Good {get;set;}
    }
}

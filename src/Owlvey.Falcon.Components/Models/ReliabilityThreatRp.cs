using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ReliabilityThreatRp
    {        
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public decimal ETTD { get; set; } // minutes
        public decimal ETTE { get; set; } // minutes
        public decimal ETTF { get; set; } // minutes        
        public decimal UserImpact { get; set; } // percentage of users affected 
        public decimal ETTFail { get; set; }  // Estimate time to fail in days per year
        public decimal ETTR { get;set; }
        public decimal IncidentsPerYear { get; set; }        
        public decimal BadMinutesPerYear { get; set; }
    }
    public class ReliabilityThreatGetRp : ReliabilityThreatRp
    {
        public int Id { get; set; }        
    }
    public class ReliabilityThreatPostRp {         
        public string Name { get; set; }       
        
    }
    public class ReliabilityThreatPutRp : ReliabilityThreatRp
    {

        
    }
}

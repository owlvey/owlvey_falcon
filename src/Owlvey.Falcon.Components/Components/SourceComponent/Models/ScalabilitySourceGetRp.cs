using Owlvey.Falcon.Core.Models.Series;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{   
    public class ScalabilitySourceGetRp
    {        
        public DatePeriodValue Period { get; set; }
        public int[] DailyTotal { get; set; }
        public int[] DailyBad { get; set; }

        public double DailyIntercept { get; set; }
        public double DailySlope { get; set; }
        public double DailyCorrelation { get; set; }        
        public double DailyR2 { get; set; }
        public List<DateInteractionModel> DailyInteractions { get; set; } = new List<DateInteractionModel>();
    }
}

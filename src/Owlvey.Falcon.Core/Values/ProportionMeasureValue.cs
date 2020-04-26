using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class ProportionMeasureValue
    {
        public decimal Proportion { get; protected set; }

        public int Total { get; protected set; }

        public int Good { get; protected set; }

        public bool HasData { get; set; } 

        public ProportionMeasureValue(decimal proportion, bool hasData=true) {
            this.Proportion = proportion;
            this.HasData = hasData;
        }
        public ProportionMeasureValue(decimal proportion, int total, int good, bool hasData = true): this(proportion, hasData)
        {
            this.Good = good;
            this.Total = total;            
        }
    }
}

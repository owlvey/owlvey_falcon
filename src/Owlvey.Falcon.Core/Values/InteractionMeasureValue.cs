using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class InteractionMeasureValue: MeasureValue
    {
        public int Total { get; set; }
        public int Good { get; set; }
        public InteractionMeasureValue(decimal value, int total, int good, bool hasData = true) : base(value, hasData)
        {
            this.Total = total;
            this.Good = good;            
        }
    }
}

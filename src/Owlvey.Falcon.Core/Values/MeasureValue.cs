using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class MeasureValue
    {
        public decimal Value { get; protected set; }

        public bool HasData { get; set; } 

        public MeasureValue(decimal value, bool hasData=true) {
            this.Value = value;
            this.HasData = hasData;
        }        
    }
}

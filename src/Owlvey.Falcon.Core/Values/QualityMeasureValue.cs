using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class QualityMeasureValue
    {        



        public decimal Quality {
            get {
                return QualityUtils.CalculateMinimum(new decimal[] { this.Availability, this.Latency });
            }
        }
        public decimal Availability { get; protected set; }
        public decimal Latency { get; protected set; }

        public bool HasData { get; protected set; }

        public QualityMeasureValue(decimal availability, decimal latency, bool hasdata = true) {            
            this.Availability = availability;
            this.Latency = latency;
            this.HasData= hasdata;
        }
        public QualityMeasureValue(decimal quality, bool hasdata = true) : this(quality, quality, hasdata)
        {
            
        }
    }
}

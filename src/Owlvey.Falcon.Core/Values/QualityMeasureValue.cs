using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class QualityMeasureValue
    {           
        public decimal Availability { get; protected set; }

        private decimal _Latency = 101;
        public decimal Latency {
            get {
                return _Latency;
            }
            protected set {
                if (value == 0) { return; }
                else { _Latency = value; }
            }
        }

        public decimal Experience { get; protected set; }

        public bool HasData { get; protected set; }

        public QualityMeasureValue(decimal availability, decimal latency, decimal experience, bool hasdata = true) {            
            this.Availability = availability;
            this.Latency = latency;
            this.HasData= hasdata;
            this.Experience = experience;
        }

        public QualityMeasureValue(bool hasdata = true)
        {
            this.Availability = 1;
            this.Latency = 0;
            this.HasData = hasdata;
            this.Experience = 1;
        }
        public QualityMeasureValue(decimal quality, bool hasdata = true) : this(quality, quality, quality, hasdata)
        {
            
        }
    }
}

using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class SLOValue
    {
        public decimal Availability { get; protected set; }
        public decimal Latency { get; protected set; }
        public decimal Experience { get; protected set; }

        public SLOValue() { }
        public SLOValue(decimal availability, decimal latency, decimal experience) {
            this.Availability = availability;
            this.Latency = latency;
            this.Experience = experience;
        }
    }
}

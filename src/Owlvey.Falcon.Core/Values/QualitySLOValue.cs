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
        public SLOValue(ServiceEntity service) {
            this.Availability = service.AvailabilitySlo;
            this.Latency = service.LatencySlo;
            this.Experience = service.ExperienceSlo;
        }
    }
}

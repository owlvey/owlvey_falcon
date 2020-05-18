using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class LatencySourceGetRp : SourceLitRp
    {
        public decimal Percentile { get; set; }
        public decimal Latency { get; set; }
        public Dictionary<string, int> Features { get; set; } = new Dictionary<string, int>();
    }
}

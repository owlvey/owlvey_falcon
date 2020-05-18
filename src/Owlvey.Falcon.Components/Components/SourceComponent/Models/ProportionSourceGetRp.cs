using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class ProportionSourceGetRp: SourceLitRp
    {
        public decimal Proportion { get; set; }
        public Dictionary<string, int> Features { get; set; } = new Dictionary<string, int>();
    }
}

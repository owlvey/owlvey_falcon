using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class InteractionSourceGetRp : SourceLitRp
    {
        public decimal Proportion { get; set; }

        public int Total { get; set; }

        public int Good { get; set; }

        public int Delta { get { return this.Total - this.Good; } }

        public Dictionary<string, int> Features { get; set; } = new Dictionary<string, int>();

    }
}

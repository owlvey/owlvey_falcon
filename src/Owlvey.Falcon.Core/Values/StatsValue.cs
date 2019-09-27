using System;
using System.Collections.Generic;
using System.Linq;


namespace Owlvey.Falcon.Core.Values
{
    public class StatsValue
    {
        public long Count { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Mean { get; set; }
        public decimal Median { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q3 { get; set; }        

        public StatsValue() {
            
        }
        public StatsValue(IEnumerable<decimal> input)
        {
            if (input.Count() == 0) {
                return;
            }
            this.Count = input.Count();
            this.Min = input.Min();
            this.Max = input.Max();
            this.Mean = input.Average();
        }
    }
}

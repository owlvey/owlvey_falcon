using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Models
{
    public class SeriesGetRp
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public IList<SeriesItemGetRp> Items { get; set; } = new List<SeriesItemGetRp>();
    }
    public class SeriesItemGetRp {
        public DateTime Date { get; protected set; }
        public decimal Availability { get; protected set; }
        public decimal Minimun { get; protected set; }
        public decimal Maximun { get; protected set; }
        public decimal Average { get; protected set; }
    }    
}

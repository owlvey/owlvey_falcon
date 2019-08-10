using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Models
{
    public class TabularGetRp
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public IList<TabularItemGetRp> Items { get; set; } = new List<TabularItemGetRp>();
    }
    public class TabularItemGetRp {
        public string Name { get; set; }
        public IList<TabularItemDayGetRp> Items { get; set; } = new List<TabularItemDayGetRp>();
    }
    public class TabularItemDayGetRp {
        public DateTime Date { get; protected set; }
        public decimal Availability { get; protected set; }
        public decimal Minimun { get; protected set; }
        public decimal Maximun { get; protected set; }
        public decimal Average { get; protected set; }
    }
}

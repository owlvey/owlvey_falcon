using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Models
{
    public class SeriesGetRp
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public IList<SeriesItemGetRp> Items { get; set; } = new List<SeriesItemGetRp>();
    }
    public class SeriesItemGetRp {
        public DateTime Date { get; protected set; }
        public decimal OAva { get; protected set; }
        public decimal OMin { get; protected set; }
        public decimal OMax { get; protected set; }
        public decimal OAve { get; protected set; }
    }

    public class MultiSerieItemGetRp {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public IList<SeriesItemGetRp> Items { get; set; } = new List<SeriesItemGetRp>();
    }

    public class MultiSeriesGetRp
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public IList<MultiSerieItemGetRp> Series { get; set; } = new List<MultiSerieItemGetRp>();
        
    }
}

using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public decimal OMin { get; protected set; }
        public decimal OMax { get; protected set; }
        public decimal OAve { get; protected set; }

        public SeriesItemGetRp() { }
        public SeriesItemGetRp(DateTime date,  decimal min , decimal max, decimal average) {
            this.Date = date;
            this.OMin = min;
            this.OMax = max;
            this.OAve = average;
        }
        public SeriesItemGetRp(DateTime date, decimal value)
        {
            this.Date = date;
            this.OMin = value;
            this.OMax = value;
            this.OAve = value;
        }

        public SeriesItemGetRp(DateTime date, IEnumerable<decimal> values)
        {
            this.Date = date;
            if (values.Count() > 0) {
                this.OMin = values.Min();
                this.OMax = values.Max();
                this.OAve = values.Average();
            }
        }


        public SeriesItemGetRp(DayPointValue value) : this(value.Date, value.Minimun, value.Maximun, value.Average) { 
        }
        

        public static List<SeriesItemGetRp> Convert(IEnumerable<DayPointValue> points)
        {
            var result = new List<SeriesItemGetRp>();
            foreach (var item in points)
            {
                result.Add(new SeriesItemGetRp(item));                
            }
            return result;
        }        
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

        public decimal SLO { get; set; }
        public IList<MultiSerieItemGetRp> Series { get; set; } = new List<MultiSerieItemGetRp>();
        
    }

    
}

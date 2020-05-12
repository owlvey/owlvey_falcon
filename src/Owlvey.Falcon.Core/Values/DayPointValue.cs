using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Values
{
    public class DayPointValue
    {        

        public DateTime Date { get; protected set; }        
        public decimal Minimun { get; protected set; }
        public decimal Maximun { get; protected set; }
        public decimal Average { get; protected set; }
        public DayPointValue(DateTime date) {
            this.Date = date;
            this.Minimun = 1;
            this.Maximun = 1;
            this.Average = 1;
        }

        public DayPointValue(DateTime date, decimal minimun, decimal maximun, decimal average) : this(date)
        {
            this.Minimun = minimun;
            this.Maximun = maximun;
            this.Average = average;            
        }

        public DayPointValue(DateTime date, IEnumerable<QualityMeasureValue> measures, Func< QualityMeasureValue, decimal> selector) : this(date)
        {
            if (measures.Count() > 0) {
                this.Minimun = measures.Select(c=>selector(c)).Min();
                this.Maximun = measures.Select(c => selector(c)).Max();
                this.Average = measures.Select(c => selector(c)).Average();
            }            
        }
        public DayPointValue(DateTime date, IEnumerable<SourceItemEntity> sourceItems) : this(date)
        {
            if (sourceItems.Count() > 0) {
                this.Minimun = sourceItems.Min(c => c.Proportion);
                this.Maximun = sourceItems.Max(c => c.Proportion);
                this.Average = sourceItems.Average(c => c.Proportion);
            }
        }
        public DayPointValue(DateTime date, IEnumerable<DayPointValue> points) : this(date)
        {        
            if (points.Count() > 0)
            {
                var minimun = points.Min(c => c.Minimun);
                var maximun = points.Max(c => c.Maximun);
                var average = QualityUtils.CalculateAverage(points.Select(c => c.Average));
            }
            
        }
    }
}

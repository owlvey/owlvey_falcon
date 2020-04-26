using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class DayMeasureValue
    {
        public DateTime Date { get; protected set; }
        public QualityMeasureValue Measure { get; protected set; }

        public DayMeasureValue(DateTime date, QualityMeasureValue measure) {
            this.Date = date;
            this.Measure = measure;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class PeriodValue
    {
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
        public PeriodValue(DateTime start, DateTime end) {

            this.Start = start.Date;
            this.End = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
        }
    }
}

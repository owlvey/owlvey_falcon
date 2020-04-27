using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class DatePeriodValue
    {
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
        public DatePeriodValue(DateTime start, DateTime end) {
            (this.Start, this.End) = DateTimeUtils.ToDate(start, end);
            this.Days = this.End.Add(TimeSpan.FromTicks(1)).Subtract(this.Start).Days;
        }
        public int Days { get; protected set; }

        public IEnumerable<DateTime> GetDates() {
            var result = new List<DateTime>();
            var pivot = this.Start;
            for (int i = 0; i < this.Days; i++) {
                result.Add(pivot.Date);
                pivot = pivot.AddDays(1);
            }
            return result;
        }
        public IEnumerable<(DateTime start, DateTime end)> GetDatesIntervals()
        {
            var result = new List<(DateTime start, DateTime end)>();
            var pivot = this.Start;
            for (int i = 0; i < this.Days; i++)
            {
                result.Add((pivot.Date, DateTimeUtils.AbsoluteEnd(pivot)));
                pivot = pivot.AddDays(1);
            }
            return result;
        }
        public (DatePeriodValue before, DatePeriodValue previous) CalculateBeforePreviousDates() {
            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(this.Start, this.End);
            return (new DatePeriodValue(bs, be), new DatePeriodValue(ps, pe));
        }
    }
}

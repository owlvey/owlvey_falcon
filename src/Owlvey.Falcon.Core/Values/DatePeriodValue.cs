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
        public DatePeriodValue(DateTime? start, DateTime? end) {
            if (!start.HasValue && !end.HasValue) {
                throw new ApplicationException("dates must have value");
            }
            (this.Start, this.End) = DateTimeUtils.ToDate(start.Value, end.Value);
            this.Days = this.End.Add(TimeSpan.FromTicks(1)).Subtract(this.Start).Days;
        }

        public DatePeriodValue(DateTime start): this(start, DateTimeUtils.AbsoluteEnd(start))
        {           
         
        }

        public int Days { get; protected set; }

        public bool IsValid() {
            return End >= Start;
        }

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

        public IEnumerable<DatePeriodValue> GetDatesPeriods()
        {
            var result = new List<DatePeriodValue>();
            var pivot = this.Start;
            for (int i = 0; i < this.Days; i++)
            {
                result.Add( new DatePeriodValue(pivot.Date, DateTimeUtils.AbsoluteEnd(pivot)));
                pivot = pivot.AddDays(1);
            }
            return result;
        }
        public (DatePeriodValue before, DatePeriodValue previous) CalculateBeforePreviousDates() {
            var (bs, be, ps, pe) = DateTimeUtils.CalculateBeforePreviousDates(this.Start, this.End);
            return (new DatePeriodValue(bs, be), new DatePeriodValue(ps, pe));
        }

        public List<DatePeriodValue> ToYearPeriods() {
            var result = new List<DatePeriodValue>();
            var year = this.Start.Year;
            for (int i = 1; i < 13; i++)
            {
                var stemp = new DateTime(year, i, 1);
                var temp = DateTime.Now;
                if (i < 12)
                {
                    temp = new DateTime(year, i + 1, 1) - TimeSpan.FromTicks(1);
                }
                else {
                    temp = new DateTime(year, i, 31);
                }
                
                result.Add(new DatePeriodValue(stemp, temp));
            }
            return result;                        
        }

        public List<DatePeriodValue> ToWeeksPeriods() {
            var result = new List<DatePeriodValue>();
            var previouus = this.Start;
            var pivot = this.Start.AddDays(7);
            while (pivot.Year == this.Start.Year) {
                result.Add(new DatePeriodValue(previouus, pivot));
                pivot += TimeSpan.FromDays(7);
                previouus += TimeSpan.FromDays(7);
            }
            return result;
        }

        public List<DatePeriodValue> ToDaysPeriods()
        {
            var result = new List<DatePeriodValue>();            
            var pivot = this.Start;
            while (pivot.Date <= this.End.Date)
            {
                result.Add(new DatePeriodValue(pivot));
                pivot += TimeSpan.FromDays(1);                
            }
            return result;
        }
        public static DatePeriodValue ToYearFromStart(DateTime start) {
            var period = new DatePeriodValue(new DateTime(start.Year, 1, 1), new DateTime(start.Year + 1, 1, 1) - TimeSpan.FromTicks(1));
            return period;
        }
    }
}

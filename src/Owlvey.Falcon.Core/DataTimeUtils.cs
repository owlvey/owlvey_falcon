using System;
namespace Owlvey.Falcon.Core
{
    public static class DateTimeUtils
    {
        public static string FormatTimeToInMinutes(double minutes) {
            if (minutes <= 0) return "00w 0d 00h 00m";
            double weeks = (minutes - (minutes % 10080D)) / 10080D;
            minutes -= (weeks * 10080D);
            double days = (minutes - (minutes % 1440D)) / 1440D;
            minutes -= (days * 1440);
            double hours = (minutes - (minutes % 60)) / 60;
            minutes -= (hours * 60);
            return string.Format("{0:00}w {1:0}d {2:00}h {3:00}m", weeks, days, hours, minutes);
        }

        public static DateTime AbsoluteStart( DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime AbsoluteEnd( DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }
        public static DateTime DatetimeToMidDate(DateTime target) {
            return target.Date.AddHours(12);
        }
        public static bool CompareDates(DateTime one, DateTime two) {
            return one.Date == two.Date;
        }
        /// <summary>
        /// Include last day
        /// </summary>
        /// <param name="end"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int DaysDiff(DateTime end, DateTime start) {
            TimeSpan span = end.AddDays(1).Subtract(start);
            return Math.Abs((int)span.TotalDays);
        }

        public static (DateTime beforeStart, DateTime beforeEnd, 
            DateTime previousStart, DateTime previousEnd) CalculateBeforePreviousDates(DateTime? start, DateTime? end) {

            if (!start.HasValue || !end.HasValue) {
                throw new ApplicationException("start or end dates are null");
            }

            var days = -1 * DaysDiff(end.Value, start.Value);

            DateTime previousEnd = start.Value.Date.AddSeconds(-1);
            DateTime previousStart = start.Value.AddDays(days).Date;

            DateTime beforeEnd = previousStart.Date.AddSeconds(-1);
            DateTime beforeStart = previousStart.AddDays(days).Date;

            return (beforeStart, beforeEnd, previousStart, previousEnd);
        }
    }
}

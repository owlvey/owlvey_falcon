using System;
namespace Owlvey.Falcon.Core
{
    public static class DateTimeUtils
    {
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
    }
}

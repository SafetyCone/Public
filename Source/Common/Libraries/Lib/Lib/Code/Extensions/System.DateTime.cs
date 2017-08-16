using System;


namespace Public.Common.Lib.Extensions
{
    // References: http://stackoverflow.com/questions/1393696/rounding-datetime-objects
    public static class DateTimeExtensions
    {
        public static string ToYYYYMMDDStr(this DateTime dateTime)
        {
            string output = String.Format(@"{0:yyyyMMdd}", dateTime);
            return output;
        }

        public static string ToHHMMSSStr(this DateTime dateTime)
        {
            string output = String.Format(@"{0:HHmmss}", dateTime);
            return output;
        }

        public static string ToYYYYMMDD_HHMMSSStr(this DateTime dateTime)
        {
            string yyyymmdd = dateTime.ToYYYYMMDDStr();
            string hhmmss = dateTime.ToHHMMSSStr();

            string output = String.Format(@"{0}-{1}", yyyymmdd, hhmmss);
            return output;
        }

        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long spanTickUnits = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(spanTickUnits * span.Ticks);
        }

        public static DateTime RoundToSecond(this DateTime date)
        {
            DateTime output = date.Round(new TimeSpan(0, 0, 1));
            return output;
        }

        public static DateTime Floor(this DateTime date, TimeSpan span)
        {
            long spanTickUnits = (date.Ticks / span.Ticks);
            return new DateTime(spanTickUnits);
        }

        public static DateTime Ceiling(this DateTime date, TimeSpan span)
        {
            long spanTickUnits = (date.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(spanTickUnits * span.Ticks);
        }
    }
}

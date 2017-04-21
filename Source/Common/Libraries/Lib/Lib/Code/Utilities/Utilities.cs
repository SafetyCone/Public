using System;
using System.Globalization;


namespace Public.Common.Lib
{
    public static class Utilities
    {
        // Note, not in the DateTimeExtensions class since this is a DateTime constructor. This would be a static method of DateTime, not an instance method, so it's not possible to create an extension method.
        #region DateTime

        public const string YYYYMMDD_Format = @"yyyyMMdd";


        public static DateTime DateTimeFromStringExact(string date, string format)
        {
            DateTime output = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            return output;
        }

        public static DateTime DateTimeFromYYYYMMDD(string yyyymmdd)
        {
            DateTime output = Utilities.DateTimeFromStringExact(yyyymmdd, Utilities.YYYYMMDD_Format);
            return output;
        }

        public static bool DateTimeTryParseFromStringExact(string date, string format, out DateTime output)
        {
            bool returnValue = DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
            return returnValue;
        }

        public static bool DateTimeTryParseFromYYYYMMDD(string yyyymmdd, out DateTime output)
        {
            bool returnValue = Utilities.DateTimeTryParseFromStringExact(yyyymmdd, Utilities.YYYYMMDD_Format, out output);
            return returnValue;
        }

        #endregion
    }
}

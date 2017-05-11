using System;


namespace Public.Common.Lib
{
    public static class DateTimeExtensions
    {
        public static string ToYYYYMMDDStr(this DateTime dateTime)
        {
            string output = String.Format(@"{0:yyyyMMdd}", dateTime);
            return output;
        }
    }
}

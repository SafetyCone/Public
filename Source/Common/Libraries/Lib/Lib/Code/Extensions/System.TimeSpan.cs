using System;


namespace Public.Common.Lib.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToStringHHMMSSFFF(this TimeSpan timeSpan)
        {
            string output = String.Format(@"{0:hh\:mm\:ss\.fff}", timeSpan);
            return output;
        }
    }
}

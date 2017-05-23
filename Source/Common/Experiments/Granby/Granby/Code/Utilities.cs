using System;
using System.IO;


namespace Public.Common.Granby
{
    public static class Utilities
    {
        public static string FormatDateTime(DateTime dateTime)
        {
            string output = String.Format(@"{0:yyyyMMdd}-{0:hh.mm.ss}", dateTime);
            return output;
        }

        public static string FormatDateTimeNow()
        {
            string output = Utilities.FormatDateTime(DateTime.Now);
            return output;
        }

        public static string GetTestFilePath(string testFileName)
        {
            string output = Path.Combine(@"..\..\..\..\Lib\Files", testFileName);
            return output;
        }

        public static string GetLogFilePath(string prefix)
        {
            string now = Utilities.FormatDateTimeNow();

            string fileName = String.Format(@"{0} {1}.txt", prefix, now);

            string output = Path.Combine(@"C:\temp\logs\Granby", fileName);
            return output;
        }
    }
}

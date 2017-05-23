using System;


namespace Public.Common.Granby.Lib.Extensions
{
    public static class IOutputStreamExtensions
    {
        public static void WriteLine(this IOutputStream outputStream)
        {
            outputStream.WriteLine(String.Empty);
        }

        public static void WriteLineAndBlankLine(this IOutputStream outputStream, string value)
        {
            outputStream.WriteLine(value);
            outputStream.WriteLine();
        }
    }
}

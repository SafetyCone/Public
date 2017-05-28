using System;


namespace Public.Common.Lib.Logging.Extensions
{
    public static class ILogExtensions
    {
        public static void WriteLineAndBlankLine(this ILog log, string value)
        {
            log.WriteLine(value);
            log.WriteLine(String.Empty);
        }
    }
}

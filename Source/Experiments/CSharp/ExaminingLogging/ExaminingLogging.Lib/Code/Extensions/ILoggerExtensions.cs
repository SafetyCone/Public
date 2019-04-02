using System;

using Microsoft.Extensions.Logging;


namespace ExaminingLogging.Lib
{
    public static class ILoggerExtensions
    {
        public static void TestAllLogLevels(this ILogger log)
        {
            log.LogTrace(@"Trace");
            log.LogDebug(@"Debug");
            log.LogInformation(@"Information");
            log.LogWarning(@"Warning");
            log.LogError(@"Error");
            log.LogCritical(@"Critical");
        }
    }
}

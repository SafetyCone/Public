using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ExaminingLogging.Lib;


namespace ExaminingLogging
{
    public class Experiments
    {
        public static void SubMain()
        {
            Experiments.GetLoggingWhenNoProviders();
        }

        /// <summary>
        /// Result: Expected, no exception and no output to console (or any other logger).
        /// What happens if you ask for a logger, but there are no logging providers setup?
        /// Expected: No exception, logging just does nothing.
        /// </summary>
        private static void GetLoggingWhenNoProviders()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<Experiments>>();

            logger.TestAllLogLevels(); // No exception, no logging to console.
        }
    }
}

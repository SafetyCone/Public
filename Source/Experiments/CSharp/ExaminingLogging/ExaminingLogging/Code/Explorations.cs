using System;

using Microsoft.Extensions.Logging;

using ExaminingLogging.Lib;


namespace ExaminingLogging
{
    public class Explorations
    {
        public static void SubMain()
        {
            Explorations.GetLoggerDirectly();
        }

        /// <summary>
        /// I want to be able to get a logger directly, without using a service collection or service provider.
        /// This shows how to make a logger factory, add a logger provider, and create a logger.
        /// </summary>
        private static void GetLoggerDirectly()
        {
            var loggerFactory = new LoggerFactory();

            loggerFactory.AddProvider(new ExtremelySimpleLoggerProvider());

            var logger = loggerFactory.CreateLogger<Explorations>();

            logger.LogInformation(@"Hello world!");
        }
    }
}

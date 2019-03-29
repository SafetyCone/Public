using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ExaminingLogging.Lib;


namespace ExaminingLogging
{
    public class Explorations
    {
        public static void SubMain()
        {
            //Explorations.GetLoggerDirectly();
            Explorations.GetLoggerFromDI();
            //Explorations.GetLoggerFilterOptions();
        }

        private static void GetLoggerFilterOptions()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            var loggerFilterOptions = serviceProvider.GetRequiredService<IOptions<LoggerFilterOptions>>();
        }

        private static void GetLoggerFromDI()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddProvider(new ExtremelySimpleLoggerProvider());

            var logger = loggerFactory.CreateLogger<Explorations>();

            logger.LogInformation(@"Hello world!", 1, 3.3, DateTime.UtcNow);
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

            logger.LogInformation(@"Hello world!", 1, 3.3, DateTime.UtcNow);
        }
    }
}

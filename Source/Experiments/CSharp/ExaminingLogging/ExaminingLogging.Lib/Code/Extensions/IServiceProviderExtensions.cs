using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ExaminingLogging.Lib
{
    public static class IServiceProviderExtensions
    {
        public static ILogger GetLogger(this IServiceProvider serviceProvider, string categoryName)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger(categoryName);
            return logger;
        }
    }
}

using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;


namespace ExaminingLogging.Lib.SimplestConsole
{
    [ProviderAlias(@"SimplestConsole")]
    public class SimplestConsoleLoggerProvider : ILoggerProvider
    {
        private ILoggerProviderConfiguration<SimplestConsoleLoggerProvider> LoggerProviderConfiguration { get; }
        private LoggerFilterOptions LoggerFilterOptions { get; }


        public SimplestConsoleLoggerProvider(ILoggerProviderConfiguration<SimplestConsoleLoggerProvider> loggerProviderConfiguration, IOptions<LoggerFilterOptions> loggerFilterOptionsOptions)
        {
            this.LoggerProviderConfiguration = loggerProviderConfiguration;

            Console.WriteLine(this.LoggerProviderConfiguration.Configuration[@"LogLevels"]);

            this.LoggerFilterOptions = loggerFilterOptionsOptions.Value;
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

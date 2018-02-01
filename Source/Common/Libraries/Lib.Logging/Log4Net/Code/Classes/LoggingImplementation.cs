using System;
using log4net.Core;



namespace Public.Common.Lib.Logging.Log4Net
{
    public class LoggingImplementation : ILoggingImplementation
    {
        public const string RootLoggerName = @"Root";


        public ILogger DefaultLogger { get; set; }


        public LoggingImplementation(string logFilePath)
        {
            log4net.Layout.SimpleLayout simpleLayout = new log4net.Layout.SimpleLayout
            {
                Header = @"Header is here..." + Environment.NewLine, // Must append the new-line to make the first log entry appears on its own line.
                Footer = @"Footer is here...",
            };

            log4net.Appender.ConsoleAppender consoleAppender = new log4net.Appender.ConsoleAppender()
            {
                Layout = simpleLayout,
            };
            consoleAppender.ActivateOptions();

            log4net.Appender.FileAppender fileAppender = new log4net.Appender.FileAppender
            {
                AppendToFile = false,
                Layout = simpleLayout,
                File = logFilePath,
            };
            fileAppender.ActivateOptions();

            log4net.Repository.ILoggerRepository defaultRepository = log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Hierarchy defaultHierarchy = (log4net.Repository.Hierarchy.Hierarchy)defaultRepository;
            defaultHierarchy.Root.AddAppender(consoleAppender);
            defaultHierarchy.Root.AddAppender(fileAppender);
            defaultHierarchy.Root.Level = log4net.Core.Level.All;
            defaultHierarchy.Configured = true;

            log4net.ILog defaultLog = log4net.LogManager.GetLogger(@"Default");

            this.DefaultLogger = new LoggerWrapper(defaultLog as LogImpl);

            //log4net.Core.ILogger defaultLogger = defaultRepository.GetLogger();
            //log4net.Repository.Hierarchy.Logger logger = (log4net.Repository.Hierarchy.Logger)defaultLogger;
            //logger.Level = log4net.Core.Level.All;

            //log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();

            //logger.AddAppender(fileAppender);

            this.DefaultLogger.Info(() => @"Using Log4Net.");
        }

        public ILogger GetDefaultLogger()
        {
            return this.DefaultLogger;
        }

        public ILogger GetLogger(ILoggerConfiguration configuration)
        {
            // Ignore configuration.

            var output = this.GetDefaultLogger();
            return output;
        }
    }
}

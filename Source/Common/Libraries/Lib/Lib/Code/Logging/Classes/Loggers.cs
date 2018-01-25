

namespace Public.Common.Lib.Logging
{
    /// <summary>
    /// Allows static access to loggers.
    /// </summary>
    public static class Loggers
    {
        /// <summary>
        /// The logging implementation to use for creating loggers when requested through the static logging facility. NOT thread safe!
        /// </summary>
        public static ILoggingImplementation Implementation { get; set; } = new DummyLoggingImplementation(); // Set to something for defaults.


        /// <summary>
        /// Get "the" logger for an application. Often we we just need a logger, any logger, and this provides that logger.
        /// </summary>
        /// <returns>The default logger for an application. This will never be null.</returns>
        public static ILogger GetDefaultLogger()
        {
            var output = Loggers.Implementation.GetDefaultLogger();
            return output;
        }

        /// <summary>
        /// Returns the input if it is non-null, otherwise returns the default logger.
        /// </summary>
        /// <param name="logger">A logger instance that may be null.</param>
        /// <returns>A non-null logger. Either the input logger if it is non-null, or the default logger.</returns>
        public static ILogger GetLoggerOrDefault(ILogger logger)
        {
            var output = logger ?? Loggers.GetDefaultLogger();
            return output;
        }

        /// <summary>
        /// Get a logger specified by a configuration.
        /// </summary>
        /// <param name="configuration">The logger configuration. This can be of any type, just as long as it is recognized by the logging implementation.</param>
        /// <returns>A configured logger.</returns>
        public static ILogger GetLogger(ILoggerConfiguration configuration)
        {
            var output = Loggers.Implementation.GetLogger(configuration);
            return output;
        }
    }
}

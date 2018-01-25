

namespace Public.Common.Lib.Logging
{
    /// <summary>
    /// A facade for a logging implementation of any type.
    /// </summary>
    public interface ILoggingImplementation
    {
        /// <summary>
        /// Get "the" logger for an application.
        /// </summary>
        /// <returns>The default logger for an application. This will never be null.</returns>
        /// <remarks>
        /// Often we just need a log, any log. This provides that log.
        /// 
        /// Note to implementers. The returned ILogger instance should never be null, allowing it to be used even when logging has not been setup.
        /// </remarks>
        ILogger GetDefaultLogger();

        /// <summary>
        /// Get a logger specified by a configuration.
        /// </summary>
        /// <param name="configuration">The logger configuration. This can be of any type, just as long as it is recognized by the logging implementation.</param>
        /// <returns>A configured logger.</returns>
        ILogger GetLogger(ILoggerConfiguration configuration);
    }
}

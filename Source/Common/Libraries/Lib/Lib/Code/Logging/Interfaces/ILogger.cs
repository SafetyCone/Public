using System;


namespace Public.Common.Lib.Logging
{
    /// <summary>
    /// Accepts log messages.
    /// </summary>
    /// <remarks>
    /// Any logging implementation can be behind the interface.
    /// </remarks>
    public interface ILogger
    {
        /// <summary>
        /// Allow clients to check whether a log message would actually be logged before going through the trouble to created it.
        /// </summary>
        bool IsEnabled(Level level);
        
        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The message object to be logged. Generally strings, but implementations can allow full machine serialization of object.</param>
        /// <remarks>
        /// While messages are usually strings, objects was used as the type for the message to allow machine serialization of complex message objects.
        /// </remarks>
        void Log(Level level, object message);
    }


    public static class ILoggerExtensions
    {
        // Message logging at a particular level.
        public static void Debug(this ILogger log, object message)
        {
            log.Log(Level.Debug, message);
        }

        public static void Info(this ILogger log, object message)
        {
            log.Log(Level.Info, message);
        }

        public static void Warn(this ILogger log, object message)
        {
            log.Log(Level.Warn, message);
        }

        public static void Error(this ILogger log, object message)
        {
            log.Log(Level.Error, message);
        }

        public static void Fatal(this ILogger log, object message)
        {
            log.Log(Level.Fatal, message);
        }

        // Methods enabling client checks for whether a log message would actually be logged before going through the trouble to create it.
        public static bool IsDebugEnabled(this ILogger log)
        {
            bool output = log.IsEnabled(Level.Debug);
            return output;
        }

        public static bool IsInfoEnabled(this ILogger log)
        {
            bool output = log.IsEnabled(Level.Info);
            return output;
        }

        public static bool IsWarnEnabled(this ILogger log)
        {
            bool output = log.IsEnabled(Level.Warn);
            return output;
        }

        public static bool IsErrorEnabled(this ILogger log)
        {
            bool output = log.IsEnabled(Level.Error);
            return output;
        }

        public static bool IsFatalEnabled(this ILogger log)
        {
            bool output = log.IsEnabled(Level.Fatal);
            return output;
        }

        // Allow clients to use delegates to specify log message generation. The delegate will only be executed if the corresponding debug level is enabled. This moves the IsEnabled check inside the logger.
        public static void Debug(this ILogger logger, Func<object> messageGenerator)
        {
            if (logger.IsDebugEnabled())
            {
                object message = messageGenerator();
                logger.Debug(message);
            }
        }

        public static void Info(this ILogger logger, Func<object> messageGenerator)
        {
            if (logger.IsInfoEnabled())
            {
                object message = messageGenerator();
                logger.Info(message);
            }
        }

        public static void Warn(this ILogger logger, Func<object> messageGenerator)
        {
            if (logger.IsWarnEnabled())
            {
                object message = messageGenerator();
                logger.Warn(message);
            }
        }

        public static void Error(this ILogger logger, Func<object> messageGenerator)
        {
            if (logger.IsErrorEnabled())
            {
                object message = messageGenerator();
                logger.Error(message);
            }
        }

        public static void Fatal(this ILogger logger, Func<object> messageGenerator)
        {
            if (logger.IsFatalEnabled())
            {
                object message = messageGenerator();
                logger.Fatal(message);
            }
        }
    }
}

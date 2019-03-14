using System;

using Microsoft.Extensions.Logging;


namespace ExaminingLogging.Lib
{
    /// <summary>
    /// Adapted from: https://app.pluralsight.com/player?course=entity-framework-core-getting-started&author=julie-lerman&name=entity-framework-core-getting-started-m4&clip=2&mode=live
    /// Somehow the original reference shown in the video is GONE!
    /// </summary>
    public class ExtremelySimpleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new ExtremelySimpleLogger();
        }

        public void Dispose()
        {
            // Do nothing, nothing to do.
        }


        private class ExtremelySimpleLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            /// <summary>
            /// Note that the logger gets to do whatever it wants with the state and its internally-tracked scope. Serialize a JSON object to a file path, no problem. Put a .NET object in an list of objects, no problem. Anything!
            /// </summary>
            /// <typeparam name="TState">
            /// Comes from the .NET Core framework (if .LogInformation() extension-methods were used), or another framework (if some other custom extension methods were used).
            /// Microsoft.Extensions.Logging.Internal.FormattedLogValues instance.
            /// </typeparam>
            /// <param name="formatter">
            /// A formatter function provided by the same framework (.NET Core or custom) as provided the <typeparamref name="TState"/>. Thus the formatter will comprehend the state because both the state and formatted came from the same framework.
            /// All .NET Core framework built-in loggers only make use of the formatter to comprehend the state input.
            /// </param>
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}

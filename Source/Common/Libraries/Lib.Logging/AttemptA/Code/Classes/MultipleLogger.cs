using System.Collections.Generic;


namespace Public.Common.Lib.Logging.AttemptA
{
    /// <summary>
    /// A class that allows treated multiple logs as a single log.
    /// </summary>
    public class MultipleLogger : ILogger
    {
        public List<ILogger> Loggers { get; } = new List<ILogger>();


        public MultipleLogger(IEnumerable<ILogger> loggers)
        {
            this.Loggers.AddRange(loggers);
        }

        public bool IsEnabled(Level level)
        {
            bool output = false;
            foreach(var logger in this.Loggers)
            {
                if(logger.IsEnabled(level))
                {
                    output = true;
                    break;
                }
            }

            return output;
        }

        public void Log(Level level, object message)
        {
            foreach (var logger in this.Loggers)
            {
                if(logger.IsEnabled(level))
                {
                    logger.Log(level, message);
                }
            }
        }
    }
}

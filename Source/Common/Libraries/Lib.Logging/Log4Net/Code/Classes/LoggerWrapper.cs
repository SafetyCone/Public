using log4net.Core;



namespace Public.Common.Lib.Logging.Log4Net
{
    public class LoggerWrapper : ILogger
    {
        public LogImpl Logger { get; }


        public LoggerWrapper(LogImpl logger)
        {
            this.Logger = logger;
        }

        public bool IsEnabled(Level level)
        {
            var log4Level = level.ToLog4Level();

            var output = this.Logger.Logger.IsEnabledFor(log4Level);
            return output;
        }

        public void Log(Level level, object message)
        {
            switch(level)
            {
                case Level.Info:
                    this.Logger.Info(message);
                    break;

                case Level.Debug:
                    this.Logger.Debug(message);
                    break;

                case Level.Warn:
                    this.Logger.Warn(message);
                    break;

                case Level.Error:
                    this.Logger.Error(message);
                    break;

                case Level.Fatal:
                    this.Logger.Fatal(message);
                    break;

                default:
                    // Do nothing.
                    break;
            }
        }
    }
}

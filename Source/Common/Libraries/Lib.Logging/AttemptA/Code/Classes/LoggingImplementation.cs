

namespace Public.Common.Lib.Logging.AttemptA
{
    public class LoggingImplementation : ILoggingImplementation
    {
        public ILogger DefaultLogger { get; set; }


        public LoggingImplementation(ILogger defaultLogger)
        {
            this.DefaultLogger = defaultLogger;
        }

        public LoggingImplementation()
            : this(DummyLogger.Instance)
        {

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

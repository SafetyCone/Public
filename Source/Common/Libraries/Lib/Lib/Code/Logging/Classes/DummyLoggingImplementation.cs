

namespace Public.Common.Lib.Logging
{
    public class DummyLoggingImplementation : ILoggingImplementation
    {
        private static ILogger Dummy { get; } = new DummyLogger();


        public ILogger GetDefaultLogger()
        {
            return DummyLoggingImplementation.Dummy;
        }

        public ILogger GetLogger(ILoggerConfiguration configuration)
        {
            // Ignore the configuration.

            return DummyLoggingImplementation.Dummy;
        }
    }
}

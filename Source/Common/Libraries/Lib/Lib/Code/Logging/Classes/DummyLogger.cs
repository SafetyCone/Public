

namespace Public.Common.Lib.Logging
{
    public class DummyLogger : ILogger
    {
        // Provide an instance to avoid the cost of multiple creation.
        public static readonly DummyLogger Instance = new DummyLogger();


        public bool IsEnabled(Level level)
        {
            return false; // Always return false so that if clients use the logger pre-check functionality, no extra work is done.
        }

        public void Log(Level level, object message)
        {
            // Do nothing.
        }
    }
}

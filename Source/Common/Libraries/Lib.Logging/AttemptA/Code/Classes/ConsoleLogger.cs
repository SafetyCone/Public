using System;


namespace Public.Common.Lib.Logging.AttemptA
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
        }

        public bool IsEnabled(Level level)
        {
            return true;   
        }

        public void Log(Level level, object message)
        {
            string line = DefaultLogEntrySerializer.Serialize(level, message);
            Console.WriteLine(line);
        }
    }
}

using System;

using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Logging
{
    public static class Utilities
    {
        public static string ToString(Level level, string message)
        {
            string output = $@"{level.ToDescription()} - {message}";
            return output;
        }

        public static TimeSpan ActionDuration(Action action)
        {
            var start = DateTime.Now;
            action();

            var duration = DateTime.Now - start;
            return duration;
        }

        public static void LogDuration(ILogger logger, TimeSpan duration, Level level, string message)
        {
            string durationMessage = $@"{duration.ToStringHHMMSSFFF()} - {message}";
            logger.Log(level, durationMessage);
        }

        public static void LogDuration(ILogger logger, StopwatchTimingNode timingNode, Level level, string message)
        {
            Utilities.LogDuration(logger, timingNode.TimingNode.ElapsedTime, level, message);
        }

        public static void ActionLog(ILogger logger, Action action, Level level, string endMessage)
        {
            action();
            logger.Log(level, endMessage);
        }

        public static void ActionLogDuration(ILogger logger, Action action, Level level, string endMessage)
        {
            var duration = Utilities.ActionDuration(action);
            Utilities.LogDuration(logger, duration, level, endMessage);
        }

        public static void ActionLog(ILogger logger, Action action, Level level, string startMessage, string endMessage)
        {
            logger.Log(level, startMessage);
            action();
            logger.Log(level, endMessage);
        }

        public static void ActionLogDuration(ILogger logger, Action action, Level level, string startMessage, string endMessage)
        {
            logger.Log(level, startMessage);
            var duration = Utilities.ActionDuration(action);
            Utilities.LogDuration(logger, duration, level, endMessage);
        }

        public static TimeSpan FunctionDuration<T>(Func<T> function, out T output)
        {
            var start = DateTime.Now;
            output = function();

            var duration = DateTime.Now - start;
            return duration;
        }

        public static T FunctionLog<T>(ILogger logger, Func<T> function, Level level, string endMessage)
        {
            T output = function();
            logger.Log(level, endMessage);

            return output;
        }

        public static T FunctionLogDuration<T>(ILogger logger, Func<T> function, Level level, string endMessage)
        {
            var duration = Utilities.FunctionDuration(function, out T output);
            Utilities.LogDuration(logger, duration, level, endMessage);

            return output;
        }

        public static T FunctionLog<T>(ILogger logger, Func<T> function, Level level, string startMessage, string endMessage)
        {
            logger.Log(level, startMessage);
            T output = function();
            logger.Log(level, endMessage);

            return output;
        }

        public static T FunctionLogDuration<T>(ILogger logger, Func<T> function, Level level, string startMessage, string endMessage)
        {
            logger.Log(level, startMessage);
            var duration = Utilities.FunctionDuration(function, out T output);
            Utilities.LogDuration(logger, duration, level, endMessage);

            return output;
        }
    }
}

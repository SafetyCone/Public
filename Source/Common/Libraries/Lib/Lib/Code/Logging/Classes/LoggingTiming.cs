

namespace Public.Common.Lib.Logging
{
    public struct LoggingTiming
    {
        public ILogger ComponentLogger { get; }
        public CallerLoggerAssumption ComponentLoggerAssumption { get; }
        public ILogger SessionLogger { get; }
        public CallerLoggerAssumption SessionLoggerAssumption { get; }
        public ITimingNode TimingNode { get; }


        public LoggingTiming(
            ILogger componentLogger = null, CallerLoggerAssumption componentLoggerAssumption = CallerLoggerAssumption.None,
            ILogger sessionLogger = null, CallerLoggerAssumption sessionLoggerAssumption = CallerLoggerAssumption.None,
            ITimingNode timingNode = null)
        {
            this.ComponentLogger = componentLogger;
            this.ComponentLoggerAssumption = componentLoggerAssumption;
            this.SessionLogger = sessionLogger;
            this.SessionLoggerAssumption = sessionLoggerAssumption;
            this.TimingNode = timingNode;
        }
    }
}

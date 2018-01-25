

namespace Public.Common.Lib.Logging
{
    /// <summary>
    /// Used to communicate from caller to callee, whether the callee should treat the provided logs as the "current" logs to use, or the "parent" logs to use in creating a child "current" log.
    /// </summary>
    public enum CallerLoggerAssumption
    {
        /// <summary>
        /// Caller makes no assumption.
        /// </summary>
        None = 0,
        /// <summary>
        /// Caller assumes callee will create its own log.
        /// </summary>
        Parent,
        /// <summary>
        /// Caller assumes callee will use the provided log.
        /// </summary>
        Current
    }
}

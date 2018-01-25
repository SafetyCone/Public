using System;


namespace Public.Common.Lib
{
    public interface ITimingNode
    {
        string Description { get; }
        /// <summary>
        /// Allow setting the elapsed time so nodes can be created before the elapsed time is known.
        /// </summary>
        TimeSpan ElapsedTime { get; set; }
    }
}

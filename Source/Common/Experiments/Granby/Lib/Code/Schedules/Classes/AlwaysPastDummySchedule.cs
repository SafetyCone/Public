using System;


namespace Public.Common.Granby.Lib
{
    /// <summary>
    /// The next event time will always be a number of seconds prior to the provided event time, i.e. the event time will always be in the past relative to the prior event time.
    /// </summary>
    public class AlwaysPastDummySchedule : ISchedule
    {
        public const int PriorNumberOfSeconds = 10;


        #region ISchedule Members

        public DateTime GetNextEventTime(DateTime priorEventTime)
        {
            DateTime output = priorEventTime.AddSeconds(-AlwaysPastDummySchedule.PriorNumberOfSeconds);
            return output;
        }

        #endregion
    }
}

using System;


namespace Public.Common.Granby.Lib
{
    /// <summary>
    /// The next event time will always be a number of seconds after the provided event time, i.e. the event time will always be in the future relative to the prior event time.
    /// </summary>
    public class AlwaysFutureDummySchedule : ISchedule
    {
        public const int FutureNumberOfSeconds = 10;


        #region ISchedule Members

        public DateTime GetNextEventTime(DateTime priorEventTime)
        {
            DateTime output = priorEventTime.AddSeconds(AlwaysFutureDummySchedule.FutureNumberOfSeconds);
            return output;
        }

        #endregion
    }
}

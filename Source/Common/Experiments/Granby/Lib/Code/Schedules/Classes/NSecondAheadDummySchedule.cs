using System;

using Public.Common.Lib.Extensions;


namespace Public.Common.Granby.Lib
{
    public class NSecondAheadDummySchedule : ISchedule
    {
        #region ISchedule Members

        public DateTime GetNextEventTime(DateTime priorEventTime)
        {
            DateTime output = priorEventTime.AddSeconds(this.NumberOfSeconds).RoundToSecond();
            return output;
        }

        #endregion


        public int NumberOfSeconds { get; set; }


        public NSecondAheadDummySchedule(int numberOfSeconds)
        {
            this.NumberOfSeconds = numberOfSeconds;
        }
    }
}

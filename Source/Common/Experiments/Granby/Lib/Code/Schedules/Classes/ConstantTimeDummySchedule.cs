using System;


namespace Public.Common.Granby.Lib
{
    public class ConstantTimeDummySchedule : ISchedule
    {
        #region Static

        public ConstantTimeDummySchedule GetConstantTimeInNSeconds(double seconds)
        {
            ConstantTimeDummySchedule output = new ConstantTimeDummySchedule(DateTime.Now.AddSeconds(seconds));
            return output;
        }

        #endregion


        #region ISchedule Members

        public DateTime GetNextEventTime(DateTime priorEventTime)
        {
            return this.EventTime;
        }

        #endregion


        public DateTime EventTime { get; set; }


        public ConstantTimeDummySchedule(DateTime time)
        {
            this.EventTime = time;
        }
    }
}

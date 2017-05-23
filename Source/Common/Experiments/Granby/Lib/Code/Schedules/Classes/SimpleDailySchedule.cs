using System;


namespace Public.Common.Granby.Lib
{
    public class SimpleDailySchedule : ISchedule
    {
        #region ISchedule Members

        public DateTime GetNextEventTime(DateTime priorEventTime)
        {
            DateTime today = DateTime.Today;
            DateTime timeIfToday = new DateTime(today.Year, today.Month, today.Day, this.TimeOfDay.Hour, this.TimeOfDay.Minute, this.TimeOfDay.Second);

            DateTime output;
            if (timeIfToday < priorEventTime)
            {
                // Already passed or equal to the specified time today, thus the next event will be at the specified time tomorrow.
                DateTime tomorrow = today.AddDays(1);
                output = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, this.TimeOfDay.Hour, this.TimeOfDay.Minute, this.TimeOfDay.Second);
            }
            else
            {
                // Else, we have not passed time specified time today, so the next event will be today.
                output = timeIfToday;
            }

            return output;
        }

        #endregion


        public DateTime TimeOfDay { get; set; }


        public SimpleDailySchedule(DateTime timeOfDay)
        {
            this.TimeOfDay = timeOfDay;
        }
    }
}

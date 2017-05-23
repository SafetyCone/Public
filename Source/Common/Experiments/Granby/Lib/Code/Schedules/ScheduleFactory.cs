using System;
using System.Collections.Generic;


namespace Public.Common.Granby.Lib
{
    public class ScheduleFactory : AegeanFactoryBase<ISchedule>
    {
        public const string ConstantTimeDummyScheduleKey = @"ConstantTimeSchedule";
        public const string NSecondsAheadDummyScheduledKey = @"NSecondsAhead";


        #region Static

        private static void AddDefaultConstructors(Dictionary<string, Func<string[], ISchedule>> constructors)
        {
            constructors.Add(ScheduleFactory.ConstantTimeDummyScheduleKey, ScheduleFactory.GetConstantTimeDummy);
            constructors.Add(ScheduleFactory.NSecondsAheadDummyScheduledKey, ScheduleFactory.GetNSecondsAheadDummy);
        }

        private static ISchedule GetConstantTimeDummy(string[] tokens)
        {
            string timeToken = tokens[1];

            DateTime time = DateTime.Parse(timeToken);

            ConstantTimeDummySchedule output = new ConstantTimeDummySchedule(time);
            return output;
        }

        private static ISchedule GetNSecondsAheadDummy(string[] tokens)
        {
            string numberOfSecondsToken = tokens[1];

            int numberOfSeconds = Int32.Parse(numberOfSecondsToken);

            NSecondAheadDummySchedule output = new NSecondAheadDummySchedule(numberOfSeconds);
            return output;
        }

        #endregion


        public ScheduleFactory()
            : base()
        {
            ScheduleFactory.AddDefaultConstructors(this.zConstructors);
        }
    }
}

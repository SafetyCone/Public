using System;
using System.Collections.Generic;


namespace Public.Common.Granby.Lib
{
    public class ScheduleFactory : AegeanFactoryBase<ISchedule>
    {
        public const string ConstantTimeDummyScheduleKey = @"ConstantTimeSchedule";
        public const string NSecondsAheadDummyScheduledKey = @"NSecondsAhead";
        public const string SimpleDailyScheduleKey = @"SimpleDaily";


        #region Static

        private static void AddDefaultConstructors(Dictionary<string, Func<string[], ISchedule>> constructors)
        {
            constructors.Add(ScheduleFactory.ConstantTimeDummyScheduleKey, ScheduleFactory.GetConstantTimeDummy);
            constructors.Add(ScheduleFactory.NSecondsAheadDummyScheduledKey, ScheduleFactory.GetNSecondsAheadDummy);
            constructors.Add(ScheduleFactory.SimpleDailyScheduleKey, ScheduleFactory.GetSimpleDailySchedule);
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

        private static ISchedule GetSimpleDailySchedule(string[] tokens)
        {
            string timeToken = tokens[1];

            DateTime time = DateTime.Parse(timeToken);

            SimpleDailySchedule output = new SimpleDailySchedule(time);
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

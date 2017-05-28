using System;

using Public.Common.Lib.Logging;


namespace Public.Common.Granby.Lib
{
    public class ScheduleTextSerializer : AegeanSerializerBase<ISchedule>
    {
        public ScheduleTextSerializer(ScheduleFactory scheduleFactory)
            : base(scheduleFactory)
        {
        }

        public ScheduleTextSerializer(ScheduleFactory scheduleFactory, ILog log)
            : base(scheduleFactory, log)
        {
        }
    }
}

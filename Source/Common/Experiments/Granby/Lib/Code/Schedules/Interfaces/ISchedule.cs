using System;


namespace Public.Common.Granby.Lib
{
    public interface ISchedule
    {
        DateTime GetNextEventTime(DateTime priorEventTime);
    }
}

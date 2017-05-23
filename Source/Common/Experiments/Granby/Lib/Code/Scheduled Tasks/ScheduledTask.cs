using System;


namespace Public.Common.Granby.Lib
{
    public class ScheduledTask
    {
        public string Name { get; set; }
        public ISchedule Schedule { get; set; }
        public ITask Task { get; set; }


        public ScheduledTask()
        {
        }

        public ScheduledTask(string name, ISchedule schedule, ITask task)
        {
            this.Name = name;
            this.Schedule = schedule;
            this.Task = task;
        }
    }
}

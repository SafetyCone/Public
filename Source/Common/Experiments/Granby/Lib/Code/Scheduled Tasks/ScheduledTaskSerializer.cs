using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Logging;
using LogLog = Public.Common.Lib.Logging.Log;


namespace Public.Common.Granby.Lib
{
    public class ScheduledTaskSerializer
    {
        public const char ScheduledTaskTokenSeparator = '|';


        #region Static

        public static List<ScheduledTask> DeserializeStatic(string filePath)
        {
            ScheduledTaskSerializer serializer = new ScheduledTaskSerializer();

            List<ScheduledTask> output = serializer.Deserialize(filePath);
            return output;
        }

        #endregion


        public ScheduleFactory ScheduleFactory { get; protected set; }
        public TaskFactory TaskFactory { get; protected set; }
        public ILog Log { get; set; }


        public ScheduledTaskSerializer(ScheduleFactory scheduleFactory, TaskFactory taskFactory, ILog log)
        {
            this.ScheduleFactory = scheduleFactory;
            this.TaskFactory = taskFactory;
            this.Log = log;
        }

        public ScheduledTaskSerializer(ILog log)
            : this(new ScheduleFactory(), new TaskFactory(), log)
        {
        }

        public ScheduledTaskSerializer()
            : this(LogLog.StringListLog())
        {
        }

        public List<ScheduledTask> Deserialize(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            List<ScheduledTask> output = new List<ScheduledTask>();
            foreach (string line in lines)
            {
                string[] tokens = line.Split(ScheduledTaskSerializer.ScheduledTaskTokenSeparator);

                string nameToken = tokens[0];
                string scheduleToken = tokens[1];
                string taskToken = tokens[2];

                try
                {
                    ISchedule schedule = this.ScheduleFactory[scheduleToken];
                    ITask task = this.TaskFactory[taskToken];

                    ScheduledTask scheduledTask = new ScheduledTask(nameToken, schedule, task);
                    output.Add(scheduledTask);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(ex.ToString());
                }
            }

            return output;
        }
    }
}

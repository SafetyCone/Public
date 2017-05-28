using System;
using System.Collections.Generic;
using System.Threading;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Logging;
using LogLog = Public.Common.Lib.Logging.Log;
using Public.Common.Lib.Logging.Extensions;

using Public.Common.Granby.Lib;


namespace Public.Common.Granby
{
    /// <summary>
    /// A basic, but production-ready scheduler.
    /// </summary>
    /// <remarks>
    /// The list of scheduled tasks is provided at construction and cannot be modified afterwards.
    /// Tasks are rescheduled upon event, making this scheduler production ready.
    /// Interactivity is one-way, provided by writing output to a constructor-provided output stream.
    /// Logging is done via a constructor-provided log.
    /// </remarks>
    public class BananaScheduler
    {
        #region Static

        private static int CompareTimeScheduledTaskTuples(Tuple<DateTime, ScheduledTask> tuple1, Tuple<DateTime, ScheduledTask> tuple2)
        {
            // Compare by event time, then by scheduled task name.
            int output = tuple1.Item1.CompareTo(tuple2.Item1);
            if (0 == output)
            {
                output = tuple1.Item2.Name.CompareTo(tuple2.Item2.Name);
            }

            return output;
        }

        public static BananaScheduler FromScheduledTasksTextFile(string filePath, IOutputStream outputStream, ILog log)
        {
            List<ScheduledTask> scheduledTasks = ScheduledTaskSerializer.DeserializeStatic(filePath);

            BananaScheduler output = new BananaScheduler(scheduledTasks, outputStream, log);
            return output;
        }

        public static BananaScheduler FromScheduledTasksTextFile(string filePath, ILog log)
        {
            List<ScheduledTask> scheduledTasks = ScheduledTaskSerializer.DeserializeStatic(filePath);

            BananaScheduler output = new BananaScheduler(scheduledTasks, MultipleOutputStream.GetDebugAndConsoleOutputStream(), log);
            return output;
        }

        public static BananaScheduler FromScheduledTasksTextFile(string filePath)
        {
            BananaScheduler output = BananaScheduler.FromScheduledTasksTextFile(filePath, LogLog.StringListLog());
            return output;
        }

        private static void SerializeScheduledActions(IEnumerable<Tuple<DateTime, ScheduledTask>> scheduledTasks, IOutputStream outputStream)
        {
            int count = 1;
            foreach (Tuple<DateTime, ScheduledTask> scheduledAction in scheduledTasks)
            {
                string line = String.Format(@"{0}. {1} - {2}", count, scheduledAction.Item1, scheduledAction.Item2.Name);
                outputStream.WriteLine(line);

                count++;
            }

            outputStream.WriteLine();
            outputStream.WriteLine();
        }

        #endregion


        private List<Tuple<DateTime, ScheduledTask>> zScheduledTasks;
        public IOutputStream OutputStream { get; private set; }
        public ILog Log { get; private set; }


        public BananaScheduler(IEnumerable<ScheduledTask> scheduledTasks, IOutputStream outputStream, ILog log)
        {
            this.zScheduledTasks = new List<Tuple<DateTime, ScheduledTask>>();

            this.OutputStream = outputStream;
            this.Log = log;

            this.ScheduleScheduledTasks(scheduledTasks);
        }

        public BananaScheduler(IEnumerable<ScheduledTask> scheduledTasks)
            : this(scheduledTasks, MultipleOutputStream.GetDebugAndConsoleOutputStream(), LogLog.StringListLog())
        {
        }

        private void ScheduleScheduledTasks(IEnumerable<ScheduledTask> scheduledTasks)
        {
            DateTime now = DateTime.Now;
            foreach (ScheduledTask scheduledTask in scheduledTasks)
            {
                DateTime eventTime = scheduledTask.Schedule.GetNextEventTime(now);
                this.zScheduledTasks.Add(new Tuple<DateTime, ScheduledTask>(eventTime, scheduledTask));
            }

            // The time-scheduled task tuple list is always kept sorted.
            this.zScheduledTasks.Sort(BananaScheduler.CompareTimeScheduledTaskTuples);
        }

        public void Run()
        {
            this.Starting();

            if (1 > this.zScheduledTasks.Count)
            {
                this.WriteLineOutputStreamAndLogBlankLine(@"No scheduled tasks available.");
                this.Exiting();
            }

            while (true)
            {
                BananaScheduler.SerializeScheduledActions(this.zScheduledTasks, this.OutputStream);

                DateTime now = DateTime.Now;

                // Scheduled tasks list is always in a sorted state as it is sorted upon addition of scheduled tasks.
                Tuple<DateTime, ScheduledTask> nextScheduledTask = this.zScheduledTasks[0];
                if (now < nextScheduledTask.Item1)
                {
                    // Wait until the next event.
                    TimeSpan timeToWait = nextScheduledTask.Item1 - now;
                    string message = String.Format(@"Time until next scheduled task is: {0}.", timeToWait);
                    this.WriteLineOutputStreamAndLogBlankLine(message);

                    int millisecondsToWait = Convert.ToInt32(timeToWait.TotalMilliseconds);
                    Thread.Sleep(millisecondsToWait);
                }

                now = DateTime.Now; // Reset now.

                // Get the list of scheduled tasks to run.
                int count = 0;
                List<Tuple<DateTime, ScheduledTask>> toRun = new List<Tuple<DateTime, ScheduledTask>>();
                foreach (Tuple<DateTime, ScheduledTask> scheduledTask in this.zScheduledTasks)
                {
                    if (scheduledTask.Item1 <= now)
                    {
                        toRun.Add(scheduledTask);
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int iScheduledTask = 0; iScheduledTask < count; iScheduledTask++)
                {
                    this.zScheduledTasks.RemoveAt(0);
                }

                // Run each scheduled task.
                List<ScheduledTask> toReschedule = new List<ScheduledTask>();
                foreach (Tuple<DateTime, ScheduledTask> scheduledTask in toRun)
                {
                    this.RunScheduledTask(scheduledTask);
                    toReschedule.Add(scheduledTask.Item2);
                }

                // Reschedule scheduled tasks.
                this.ScheduleScheduledTasks(toReschedule);
            }
        }

        private void RunScheduledTask(Tuple<DateTime, ScheduledTask> scheduledTask)
        {
            string now = Utilities.FormatDateTimeNow();
            string scheduledTime = Utilities.FormatDateTime(scheduledTask.Item1);
            string message = String.Format(@"Running scheduled task: {0}. Scheduled time: {1}, actual time: {2}.", scheduledTask.Item2.Name, scheduledTask.Item1, scheduledTime, now);
            this.WriteLineOutputStreamAndLogBlankLine(message);

            try
            {
                scheduledTask.Item2.Task.Run();
            }
            catch (Exception ex)
            {
                this.WriteLineOutputStreamAndLog(@"ERROR");
                this.OutputStream.WriteLineAndBlankLine(ex.Message);
                this.Log.WriteLineAndBlankLine(ex.ToString());
            }
        }

        private void Starting()
        {
            string now = Utilities.FormatDateTimeNow();
            string message = String.Format(@"Starting Banana Scheduler - {0}.", now);
            this.WriteLineOutputStreamAndLogBlankLine(message);
        }

        private void Exiting()
        {
            string now = Utilities.FormatDateTimeNow();
            string message = String.Format(@"Exiting Banana Scheduler - {0}.", now);
            this.WriteLineOutputStreamAndLog(message);
        }

        private void WriteLineOutputStreamAndLog(string value)
        {
            this.OutputStream.WriteLine(value);
            this.Log.WriteLine(value);
        }

        private void WriteLineOutputStreamAndLog()
        {
            this.WriteLineOutputStreamAndLog(String.Empty);
        }

        private void WriteLineOutputStreamAndLogBlankLine(string value)
        {
            this.WriteLineOutputStreamAndLog(value);
            this.WriteLineOutputStreamAndLog();
        }
    }
}

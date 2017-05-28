using System;
using System.Collections.Generic;
using System.Threading;

using Public.Common.Lib.IO;
using Public.Common.Lib.Logging;

using Public.Common.Granby.Lib;


namespace Public.Common.Granby
{
    /// <summary>
    /// The most basic, but intuitive, scheduler.
    /// </summary>
    /// <remarks>
    /// The schedule of action-time tuples is pre-built and provided at construction and cannot be modified afterwards.
    /// Tasks are not rescheduled upon event. Restart the scheduler (this prevents this first example from being production-ready)
    /// Interactivity is one-way, provided by writing output to a constructor-provided output stream.
    /// Logging is done via a constructor-provided log.
    /// </remarks>
    public class AppleScheduler
    {
        #region Static

        public static ILog GetDefaultFileLog()
        {
            string logFilePath = AppleScheduler.GetDefaultLogFilePath();
            
            Log output = new Log(logFilePath);
            return output;
        }

        public static string GetDefaultLogFilePath()
        {
            string now = Utilities.FormatDateTimeNow();

            string output = String.Format(@"C:\temp\Apple Scheduler - {0}.txt", now);
            return output;
        }

        private static int TaskTimeTupleComparison(Tuple<DateTime, Action> scheduledTask1, Tuple<DateTime, Action> scheduledTask2)
        {
            int output = scheduledTask1.Item1.CompareTo(scheduledTask2.Item1);
            return output;
        }

        /// <remarks>
        /// The list of scheduled tasks is returned so as to keep referenced to the scheduled tasks alive. This is required as each action is just the scheduled task's task' run method.
        /// </remarks>
        public static Tuple<AppleScheduler, List<ScheduledTask>> FromScheduledTasksTextFile(string filePath)
        {
            List<ScheduledTask> scheduledTasks = ScheduledTaskSerializer.DeserializeStatic(filePath);

            DateTime now = DateTime.Now;

            List<Tuple<DateTime, Action>> scheduledActions = new List<Tuple<DateTime,Action>>();
            foreach(ScheduledTask scheduledTask in scheduledTasks)
            {
                DateTime nextEventTime = scheduledTask.Schedule.GetNextEventTime(now);
                Action action = scheduledTask.Task.Run;

                scheduledActions.Add(new Tuple<DateTime,Action>(nextEventTime, action));
            }

            AppleScheduler output = new AppleScheduler(scheduledActions);

            return new Tuple<AppleScheduler,List<ScheduledTask>>(output, scheduledTasks);
        }

        private static void SerializeScheduledActions(IEnumerable<Tuple<DateTime, Action>> scheduledActions, IOutputStream outputStream)
        {
            outputStream.WriteLine(String.Empty);

            int count = 1;
            foreach (Tuple<DateTime, Action> scheduledAction in scheduledActions)
            {
                string line = String.Format(@"{0}. {1} - {2}", count, scheduledAction.Item1, scheduledAction.Item2);
                outputStream.WriteLine(line);

                count++;
            }

            outputStream.WriteLine(String.Empty);
            outputStream.WriteLine(String.Empty);
        }

        #endregion


        private List<Tuple<DateTime, Action>> zScheduledActions;
        private IOutputStream zOutputStream;
        private ILog zLog;


        public AppleScheduler(IEnumerable<Tuple<DateTime, Action>> scheduledActions, IOutputStream outputStream, ILog log)
        {
            this.zScheduledActions = new List<Tuple<DateTime, Action>>(scheduledActions);
            this.zOutputStream = outputStream;
            this.zLog = log;
        }

        public AppleScheduler(IEnumerable<Tuple<DateTime, Action>> scheduledActions)
            : this(scheduledActions, MultipleOutputStream.GetDebugAndConsoleOutputStream(), Log.StringListLog())
        {
        }

        public AppleScheduler(IEnumerable<Tuple<DateTime, Action>> scheduledActions, string logFilePath)
            : this(scheduledActions, MultipleOutputStream.GetDebugAndConsoleOutputStream(), new Log(logFilePath))
        {
        }

        public void Run()
        {
            string message;
            string now = Utilities.FormatDateTimeNow();
            message = String.Format(@"Starting Apple Scheduler - {0}.", now);
            this.WriteLineOutputStreamAndLog(message);
            this.WriteLineOutputStreamAndLog(String.Empty);

            this.zScheduledActions.Sort(AppleScheduler.TaskTimeTupleComparison);
            
            while (0 < this.zScheduledActions.Count)
            {
                AppleScheduler.SerializeScheduledActions(this.zScheduledActions, this.zOutputStream);

                Tuple<DateTime, Action> firstActionTimeTuple = this.zScheduledActions[0];
                this.zScheduledActions.RemoveAt(0);

                string firstActionTime = Utilities.FormatDateTime(firstActionTimeTuple.Item1);
                message = String.Format(@"Scheduled action found. Scheduled time: {0}.", firstActionTime);
                this.WriteLineOutputStreamAndLog(message);

                TimeSpan timeToWait = firstActionTimeTuple.Item1 - DateTime.Now;
                message = String.Format(@"Time to wait is: {0}.", timeToWait);
                this.WriteLineOutputStreamAndLog(message);

                int millisecondsToWait = Convert.ToInt32(timeToWait.TotalMilliseconds);
                if (0 < millisecondsToWait)
                {
                    Thread.Sleep(millisecondsToWait);
                }

                now = Utilities.FormatDateTimeNow();
                message = String.Format(@"Running scheduled action. Scheduled time: {0}, actual time: {1}.", firstActionTime, now);
                this.WriteLineOutputStreamAndLog(message);
                this.WriteLineOutputStreamAndLog(String.Empty);

                // No exception handling or logging of exceptions.
                firstActionTimeTuple.Item2();
            }
            this.WriteLineOutputStreamAndLog(String.Empty);

            // There are no tasks to be run, return.
            now = Utilities.FormatDateTimeNow();
            message = String.Format(@"No scheduled actions available. Exiting scheduler at time = {0}.", now);
            this.WriteLineOutputStreamAndLog(message);
        }

        private void WriteLineOutputStreamAndLog(string value)
        {
            this.zOutputStream.WriteLine(value);
            this.zLog.WriteLine(value);
        }
    }
}

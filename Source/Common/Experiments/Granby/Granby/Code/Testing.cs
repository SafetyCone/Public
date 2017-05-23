using System;
using System.Collections.Generic;

using Public.Common.Granby.Lib;


namespace Public.Common.Granby
{
    public static class Testing
    {
        public const string ScheduleExamplesFileName = @"Schedule Examples.txt";
        public const string TaskExamplesFileName = @"Task Examples.txt";
        public const string ScheduledTaskExamplesFileName = @"Scheduled Task Examples.txt";
        public const string ExceptionScheduledTaskExamplesFileName = @"Exception Scheduled Task Examples.txt";
        public const string AppleTestScheduledTasks1FileName = @"Apple Test Scheduled Tasks 1.txt";
        public const string BananaTestScheduledTasks1FileName = @"Banana Test Scheduled Tasks 1.txt";
        public const string BananaTestScheduledTasks2FileName = @"Banana Test Scheduled Tasks 2.txt";



        public static void SubMain(string[] args)
        {
            //Testing.TestBananaExceptionHandling();
            Testing.TestBananaScheduler();
            //Testing.TestAppleScheduler();
            //Testing.TestScheduledTaskDeserialization();
            //Testing.TestTaskDeserialization();
            //Testing.TestScheduleDeserialization();
        }

        private static void TestBananaExceptionHandling()
        {
            string filePath = Utilities.GetTestFilePath(Testing.ExceptionScheduledTaskExamplesFileName);
            Testing.RunBanana(filePath);   
        }

        private static void TestBananaScheduler()
        {
            //string filePath = Utilities.GetTestFilePath(Testing.BananaTestScheduledTasks1FileName);
            string filePath = Utilities.GetTestFilePath(Testing.BananaTestScheduledTasks2FileName);
            Testing.RunBanana(filePath);   
        }

        private static void RunBanana(string inputFilePath)
        {
            string logFilePath = Utilities.GetLogFilePath(@"Banana Scheduler");
            Log log = new Log(new FileOutputStream(logFilePath));

            BananaScheduler bananaScheduler = BananaScheduler.FromScheduledTasksTextFile(inputFilePath, log);

            bananaScheduler.Run();
        }

        private static void TestAppleScheduler()
        {
            string filePath = Utilities.GetTestFilePath(Testing.AppleTestScheduledTasks1FileName);
            Tuple<AppleScheduler, List<ScheduledTask>> appleScheduler = AppleScheduler.FromScheduledTasksTextFile(filePath);

            appleScheduler.Item1.Run();
        }

        private static void TestScheduledTaskDeserialization()
        {
            ScheduledTaskSerializer serializer = new ScheduledTaskSerializer();

            string filePath = Utilities.GetTestFilePath(Testing.ScheduledTaskExamplesFileName);
            List<ScheduledTask> scheduledTasks = serializer.Deserialize(filePath);
        }

        private static void TestTaskDeserialization()
        {
            TaskFactory factory = new TaskFactory();
            TaskTextSerializer serializer = new TaskTextSerializer(factory);

            string filePath = Utilities.GetTestFilePath(Testing.TaskExamplesFileName);
            List<ITask> tasks = serializer.Deserialize(filePath);
        }

        private static void TestScheduleDeserialization()
        {
            ScheduleFactory factory = new ScheduleFactory();
            ScheduleTextSerializer serializer = new ScheduleTextSerializer(factory);

            string filePath = Utilities.GetTestFilePath(Testing.ScheduleExamplesFileName);
            List<ISchedule> schedules = serializer.Deserialize(filePath);

            List<DateTime> eventTimes = new List<DateTime>();
            DateTime now = DateTime.Now;
            foreach (ISchedule schedule in schedules)
            {
                DateTime eventTime = schedule.GetNextEventTime(now);
                eventTimes.Add(eventTime);
            }
        }
    }
}

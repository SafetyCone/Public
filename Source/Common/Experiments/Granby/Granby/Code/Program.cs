using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Granby.Lib;


namespace Public.Common.Granby
{
    class Program
    {
        private static void Main(string[] args)
        {
            Program.SubMain(args);
            //Program.RunAppleScheduler();
            //Testing.SubMain(args);
        }

        public static void SubMain(string[] args)
        {
            TextWriter outputStream = Console.Out;

            Configuration config;
            if (Configuration.TryParseArguments(out config, outputStream, args))
            {
                try
                {
                    Program.RunBananaScheduler(config.ScheduledTasksFilePath);
                }
                catch (Exception ex)
                {
                    outputStream.WriteLine(ex.Message);
                }
            }
        }

        private static void RunBananaScheduler(string inputFilePath)
        {
            string logFilePath = Utilities.GetLogFilePath(@"Banana Scheduler");
            string logDirectoryPath = Path.GetDirectoryName(logFilePath);
            if(!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }

            Log log = new Log(new FileOutputStream(logFilePath));

            BananaScheduler bananaScheduler = BananaScheduler.FromScheduledTasksTextFile(inputFilePath, log);

            bananaScheduler.Run();
        }

        private static void RunAppleScheduler()
        {
            List<Tuple<DateTime, Action>> scheduledActions = new List<Tuple<DateTime, Action>>();

            MessageBoxDummyTask messageBox = new MessageBoxDummyTask(@"Hello world!");
            DateTime now = DateTime.Now;
            scheduledActions.Add(new Tuple<DateTime, Action>(now.AddSeconds(5), () => messageBox.Run()));
            scheduledActions.Add(new Tuple<DateTime, Action>(now.AddSeconds(10), () => messageBox.Run()));
            scheduledActions.Add(new Tuple<DateTime, Action>(now.AddSeconds(15), () => messageBox.Run()));

            AppleScheduler apple = new AppleScheduler(scheduledActions);
            apple.Run();
        }

        private static IOutputStream GetFullOutputStreamSet()
        {
            string outputFilePath = Program.GetOutputFilePath();

            FileOutputStream file = new FileOutputStream(outputFilePath);
            ConsoleOutputStream console = new ConsoleOutputStream();
            DebugOutputStream debug = new DebugOutputStream();

            MultipleOutputStream output = new MultipleOutputStream(new IOutputStream[] { debug, console, file });
            return output;
        }

        private static string GetOutputFilePath()
        {
            string now = Utilities.FormatDateTimeNow();

            string output = String.Format(@"C:\temp\Granby - {0}.txt", now);
            return output;
        }
    }
}

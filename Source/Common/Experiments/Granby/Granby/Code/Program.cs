using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Logging;

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
            ConsoleOutputStream console = new ConsoleOutputStream();
            DebugOutputStream debug = new DebugOutputStream();
            MultipleOutputStream debugAndConsole = new MultipleOutputStream(new IOutputStream[] { console, debug });
            debugAndConsole.WriteLineAndBlankLine(Constants.ProgramName);

            string logFilePath = Log.GetAndEnsureDefaultLogFilePath(Constants.ProgramName);
            debugAndConsole.WriteLine(@"Log file path:");
            debugAndConsole.WriteLineAndBlankLine(logFilePath);

            Log log = new Log(logFilePath);
            MultipleOutputStream logAndConsole = new MultipleOutputStream(new IOutputStream[] { log.OutputStream, console });

            Configuration config;
            if (Configuration.TryParseArguments(out config, logAndConsole, args))
            {
                try
                {
                    Program.RunBananaScheduler(config.ScheduledTasksFilePath, debugAndConsole, log);
                }
                catch (Exception ex)
                {
                    logAndConsole.WriteLine(@"ERROR running Banana Scheduler.");
                    logAndConsole.WriteLine(ex.Message);
                }
            }

            logAndConsole.WriteLineAndBlankLine(@"Exiting program...");
        }

        private static void RunBananaScheduler(string inputFilePath, IOutputStream outputStream, ILog log)
        {
            BananaScheduler bananaScheduler = BananaScheduler.FromScheduledTasksTextFile(inputFilePath, outputStream, log);
            bananaScheduler.Run();
        }

        private static void RunBananaScheduler(string inputFilePath, IOutputStream outputStream)
        {
            string logFilePath = Utilities.GetLogFilePath(@"Banana Scheduler");
            string logDirectoryPath = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }

            Log log = new Log(new FileOutputStream(logFilePath));

            Program.RunBananaScheduler(inputFilePath, outputStream, log);
        }

        private static void RunBananaScheduler(string inputFilePath)
        {
            IOutputStream debugAndConsole = MultipleOutputStream.GetDebugAndConsoleOutputStream();

            Program.RunBananaScheduler(inputFilePath, debugAndConsole);
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

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ExaminingConsole.Explorations
{
    /// <summary>
    /// 
    /// The mechanism by which console key-presses are realized is by polling the <see cref="Console.KeyAvailable"/> property in a while loop.
    /// After this polling process, normal event notification can occur.
    /// 
    /// Adapted from: https://stackoverflow.com/questions/5891538/listen-for-key-press-in-net-console-app
    /// Which was adapted from: https://www.pluralsight.com/courses/building-dotnet-console-applications-csharp
    /// </summary>
    public class FileProcessor
    {
        #region Static

        public static void SubMain()
        {
            Console.WriteLine("Press CTRL+C or CTRL+Break to Exit");
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            };


            var taskKeys = new Task(ReadKeys);
            var taskProcessFiles = new Task(ProcessFiles);

            taskKeys.Start();
            taskProcessFiles.Start();

            //var tasks = new[] { taskKeys }; // Wait for only the key-press task to allow the key-press task to ALSO quit the application.
            var tasks = new[] { taskKeys, taskProcessFiles }; // Wait for both tasks to make the cancel-key press the only way to quit the application.
            Task.WaitAll(tasks);
        }

        private static void ProcessFiles()
        {
            var files = Enumerable.Range(1, 100).Select(n => "File" + n + ".txt");

            var taskBusy = new Task(BusyIndicator);
            taskBusy.Start();

            foreach (var file in files)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Procesing file {0}", file);
            }
        }

        private static void BusyIndicator()
        {
            var busy = new ConsoleBusyIndicator();
            busy.UpdateProgress();
        }

        private static void ReadKeys()
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (!Console.KeyAvailable && key.Key != ConsoleKey.Escape)
            {
                key = Console.ReadKey(true); // Blocks the thread just like Read() or ReadLine().

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        Console.WriteLine("UpArrow was pressed");
                        break;
                    case ConsoleKey.DownArrow:
                        Console.WriteLine("DownArrow was pressed");
                        break;

                    case ConsoleKey.RightArrow:
                        Console.WriteLine("RightArrow was pressed");
                        break;

                    case ConsoleKey.LeftArrow:
                        Console.WriteLine("LeftArrow was pressed");
                        break;

                    case ConsoleKey.Escape:
                        break;

                    default:
                        if (Console.CapsLock && Console.NumberLock)
                        {
                            Console.WriteLine(key.KeyChar);
                        }
                        break;
                }
            }
        }

        #endregion


        internal class ConsoleBusyIndicator
        {
            int _currentBusySymbol;

            public char[] BusySymbols { get; set; }

            public ConsoleBusyIndicator()
            {
                BusySymbols = new[] { '|', '/', '-', '\\' };
            }
            public void UpdateProgress()
            {
                while (true)
                {
                    Thread.Sleep(100);
                    var originalX = Console.CursorLeft;
                    var originalY = Console.CursorTop;

                    Console.Write(BusySymbols[_currentBusySymbol]);

                    _currentBusySymbol++;

                    if (_currentBusySymbol == BusySymbols.Length)
                    {
                        _currentBusySymbol = 0;
                    }

                    Console.SetCursorPosition(originalX, originalY);
                }
            }
        }
    }
}

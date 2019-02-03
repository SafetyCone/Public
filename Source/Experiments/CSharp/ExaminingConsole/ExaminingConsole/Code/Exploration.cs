using System;
using System.IO;
using System.Threading;


namespace ExaminingConsole
{
    public static class Exploration
    {
        public static void SubMain()
        {
            //Exploration.RedirectStandardInput();
            //Exploration.TryConsoleKeyEvents();

            Explorations.FileProcessor.SubMain();
        }

        private static void TryConsoleKeyEvents()
        {
            Console.WriteLine("Press CTRL+C or CTRL+Break to Exit");
            ConsoleKeyEvents.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            };

            ConsoleKeyEvents.Press += (sender, e) =>
            {
                Console.WriteLine($@"You pressed {e.ConsoleKeyInfo.Key}");
                Console.WriteLine(e.ConsoleKeyInfo.KeyChar);
            };

            ConsoleKeyEvents.InterceptKey = false;

            while (true) ;
        }

        /// <summary>
        /// I want to get the key-presses to the console as events.
        /// </summary>
        private static void RedirectStandardInput()
        {
            Console.WriteLine($@"Is input redirected: {Console.IsInputRedirected}");

            var input = Console.OpenStandardInput();

            Console.WriteLine($@"Is input redirected: {Console.IsInputRedirected}");

            //Console.can

            Thread.Sleep(1000);
        }
    }
}

using System;
using System.Threading;


namespace ExaminingConsole
{
    /// <summary>
    /// Allows listening to console key-presses.
    /// Can use a static thread since the thread will only be created if the class is referenced.
    /// </summary>
    public static class ConsoleKeyEvents
    {
        public static event EventHandler<ConsoleKeyPressEventArgs> Press;
        public static event EventHandler<ConsoleCancelEventArgs> CancelKeyPress;


        /// <summary>
        /// Determines whether key-presses are displayed to the console (false) or not (true).
        /// By default do not display the pressed-key in the console. Let Press-event clients decide if they want to do that.
        /// </summary>
        public static bool InterceptKey { get; set; } = true;


        static ConsoleKeyEvents()
        {
            Console.CancelKeyPress += ConsoleKeyEvents.Console_CancelKeyPress;

            var threadStart = new ThreadStart(ConsoleKeyEvents.ReadKeys);
            var thread = new Thread(threadStart);
            thread.Start();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ConsoleKeyEvents.CancelKeyPress?.Invoke(sender, e);
        }

        private static void ReadKeys()
        {
            while(!Console.KeyAvailable)
            {
                // Blocks the thread just like Read() or ReadLine().
                var key = Console.ReadKey(ConsoleKeyEvents.InterceptKey);

                ConsoleKeyEvents.Press?.Invoke(null, new ConsoleKeyPressEventArgs(key));
            }
        }
    }
}

using System;


namespace ExaminingConsole
{
    public class ConsoleKeyPressEventArgs
    {
        public ConsoleKeyInfo ConsoleKeyInfo { get; }


        public ConsoleKeyPressEventArgs(ConsoleKeyInfo consoleKeyInfo)
        {
            this.ConsoleKeyInfo = consoleKeyInfo;
        }
    }
}

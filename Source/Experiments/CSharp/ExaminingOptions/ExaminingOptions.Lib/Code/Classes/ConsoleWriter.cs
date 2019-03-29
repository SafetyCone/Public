using System;

using Microsoft.Extensions.Options;


namespace ExaminingOptions.Lib
{
    public class ConsoleWriter : IConsoleWriter
    {
        private IOptions<ConsoleWriterOptions> Options { get; }


        public ConsoleWriter(IOptions<ConsoleWriterOptions> options)
        {
            this.Options = options;
        }

        public void Write()
        {
            Console.WriteLine(this.Options.Value.Message);
        }
    }
}

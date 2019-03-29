using System;

using Microsoft.Extensions.Options;


namespace ExaminingOptions.Lib
{
    public class ConsoleWriterExtraConfigurator : IConfigureOptions<ConsoleWriterOptions>
    {
        public void Configure(ConsoleWriterOptions options)
        {
            options.Message += @" YO!";
        }
    }
}

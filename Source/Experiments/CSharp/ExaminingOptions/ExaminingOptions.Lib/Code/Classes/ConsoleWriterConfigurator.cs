using System;

using Microsoft.Extensions.Options;


namespace ExaminingOptions.Lib
{
    public class ConsoleWriterConfigurator : IConfigureOptions<ConsoleWriterOptions>
    {
        public void Configure(ConsoleWriterOptions options)
        {
            options.Message = $@"Message from {nameof(ConsoleWriterConfigurator)}";
        }
    }
}

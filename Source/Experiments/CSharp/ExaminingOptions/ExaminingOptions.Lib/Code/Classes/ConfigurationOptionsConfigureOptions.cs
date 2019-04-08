using System;

using Microsoft.Extensions.Options;


namespace ExaminingOptions.Lib
{
    public class ConfigurationOptionsConfigureOptions : IConfigureOptions<ConfigurationOptions>
    {
        public void Configure(ConfigurationOptions options)
        {
            Console.WriteLine(@"HERE!");
        }
    }
}

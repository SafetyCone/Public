using System;

using Microsoft.Extensions.Logging;


namespace ExaminingLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            Explorations.SubMain();
        }

        private static void TypesOfInterest()
        {
            string temp;

            temp = nameof(ILoggerFactory);
            temp = nameof(LoggerFactory);

            temp = nameof(ILoggerProvider);


            temp = nameof(LoggerFilterOptions);
            temp = nameof(LoggerFilterRule);

            temp = nameof(FilterLoggingBuilderExtensions);
            //temp = nameof(LoggingBuilderExtensions);

            Console.WriteLine(temp);
        }
    }
}

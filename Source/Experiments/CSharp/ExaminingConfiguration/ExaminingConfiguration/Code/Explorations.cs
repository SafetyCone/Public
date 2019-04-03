using System;

using Microsoft.Extensions.Configuration;

using ExaminingConfiguration.Lib;


namespace ExaminingConfiguration
{
    public static class Explorations
    {
        public static void SubMain()
        {
            Explorations.ExamineEmptyConfiguration();
        }

        private static void ExamineEmptyConfiguration()
        {
            var configuration = new ConfigurationBuilder().Build();

            var isEmpty = configuration.IsEmpty();

            Console.WriteLine($@"Configuration is empty: {isEmpty}");
        }
    }
}

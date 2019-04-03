using System;

using Microsoft.Extensions.Configuration;

using ExaminingConfiguration.Lib;


namespace ExaminingConfiguration
{
    public static class Experiments
    {
        public static void SubMain()
        {
            //Experiments.ResultOfNonExistentConfigurationKey();
            Experiments.ResultSectionOfNonExistentConfigurationKey();
        }

        /// <summary>
        /// Result: Expected, an empty configuration section.
        /// What happens if you ask for a configuration section for a key that does not exist?
        /// Expected: An empty configuration section (a configuration section with no children) as stated here (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2#getsection).
        /// </summary>
        private static void ResultSectionOfNonExistentConfigurationKey()
        {
            var configuration = new ConfigurationBuilder().Build(); // Empty configuration.

            var configurationSection = configuration.GetSection(@"Logging:Stuff:Stuff");

            var isNull = configurationSection == null;
            var isEmpty = configurationSection.IsEmpty();

            Console.WriteLine($@"Configuration section: {configurationSection}");
            Console.WriteLine($@"Is <null>: {isNull}");
            Console.WriteLine($@"Is empty: {isEmpty}");
        }

        /// <summary>
        /// Result: Expected, null.
        /// What happens if you ask for a configuration key (path) that does not exist?
        /// Expected: null will be returned if a key does not exist.
        /// </summary>
        private static void ResultOfNonExistentConfigurationKey()
        {
            var configuration = new ConfigurationBuilder().Build(); // Empty configuration.

            var configurationSection = configuration[@"Logging:Stuff:Stuff"];

            Console.WriteLine($@"Configuration section: {(configurationSection ?? @"<null>")}"); ;
        }
    }
}

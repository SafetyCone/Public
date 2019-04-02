using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ExaminingConfiguration.Lib
{
    public static class IConfigurationSectionExtensions
    {
        public static void DescribeUsing(this IConfigurationSection configurationSection, Action<string> descriptionSink)
        {
            descriptionSink($@"{configurationSection.Path}: {configurationSection.Value}");
        }

        public static void DescibeConfiguration(this IEnumerable<IConfigurationSection> configurationSections, Action<string> descriptionSink)
        {
            foreach (var configurationSection in configurationSections)
            {
                configurationSection.DescribeUsing(descriptionSink);
            }
        }

        public static void DescribeServices(this IEnumerable<IConfigurationSection> services, StreamWriter writer)
        {
            services.DescibeConfiguration(x => writer.WriteLine(x));
        }

        public static void DescribeServices(this IEnumerable<IConfigurationSection> services, ILogger logger)
        {
            services.DescibeConfiguration(x => logger.LogInformation(x));
        }
    }
}

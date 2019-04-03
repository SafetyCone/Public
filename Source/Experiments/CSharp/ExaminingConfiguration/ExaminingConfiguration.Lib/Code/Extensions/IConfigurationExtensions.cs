using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ExaminingConfiguration.Lib
{
    public static class IConfigurationExtensions
    {
        public static bool IsEmpty(this IConfiguration configuration)
        {
            var children = configuration.GetChildren();

            var childCount = children.Count();

            var isEmpty = childCount < 1;
            return isEmpty;
        }

        public static IEnumerable<IConfigurationSection> GetAllLeafConfigurationSections(this IConfiguration configuration)
        {
            foreach (var configurationSection in configuration.GetChildren())
            {
                foreach (var childConfigurationSection in configurationSection.GetAllLeafConfigurationSections())
                {
                    yield return childConfigurationSection;
                }
            }
        }

        public static void DescribeConfiguration(this IConfiguration configuration, Action<string> descriptionSink)
        {
            configuration.GetAllLeafConfigurationSections().DescribeConfiguration(descriptionSink);
        }

        public static void DescribeConfiguration(this IConfiguration configuration, StreamWriter writer)
        {
            configuration.DescribeConfiguration(x => writer.WriteLine(x));
        }

        public static void DescribeConfiguration(this IConfiguration configuration, ILogger logger)
        {
            configuration.DescribeConfiguration(x => logger.LogInformation(x));
        }
    }
}

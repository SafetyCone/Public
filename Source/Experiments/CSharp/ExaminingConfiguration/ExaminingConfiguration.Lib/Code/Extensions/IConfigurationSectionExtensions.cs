using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ExaminingConfiguration.Lib
{
    public static class IConfigurationSectionExtensions
    {
        public static IEnumerable<IConfigurationSection> GetAllLeafConfigurationSections(this IConfigurationSection configurationSection)
        {
            var children = configurationSection.GetChildren();
            if (children.Count() == 0)
            {
                yield return configurationSection;
            }
            else
            {
                foreach (var child in children)
                {
                    var childConfigurationSections = child.GetAllLeafConfigurationSections();
                    foreach (var childConfigurationSection in childConfigurationSections)
                    {
                        yield return childConfigurationSection;
                    }
                }
            }
        }

        public static void DescribeUsing(this IConfigurationSection configurationSection, Action<string> descriptionSink)
        {
            descriptionSink($@"{configurationSection.Path} - {configurationSection.Value}");
        }

        public static void DescribeConfiguration(this IEnumerable<IConfigurationSection> configurationSections, Action<string> descriptionSink)
        {
            foreach (var configurationSection in configurationSections)
            {
                configurationSection.DescribeUsing(descriptionSink);
            }
        }

        public static void DescribeConfiguration(this IEnumerable<IConfigurationSection> services, StreamWriter writer)
        {
            services.DescribeConfiguration(x => writer.WriteLine(x));
        }

        public static void DescribeConfiguration(this IEnumerable<IConfigurationSection> services, ILogger logger)
        {
            services.DescribeConfiguration(x => logger.LogInformation(x));
        }
    }
}

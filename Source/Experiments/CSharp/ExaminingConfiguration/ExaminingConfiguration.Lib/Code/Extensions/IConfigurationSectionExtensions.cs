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
        public static void DescribeUsing(this IConfigurationSection configurationSection, Action<string> descriptionSink)
        {
            descriptionSink($@"{configurationSection.Path} - {configurationSection.Value}");
        }

        public static void DescibeConfiguration(this IEnumerable<IConfigurationSection> configurationSections, Action<string> descriptionSink)
        {
            foreach (var configurationSection in configurationSections)
            {
                configurationSection.DescribeUsing(descriptionSink);
            }
        }

        public static void DescibeConfiguration(this IEnumerable<IConfigurationSection> services, StreamWriter writer)
        {
            services.DescibeConfiguration(x => writer.WriteLine(x));
        }

        public static void DescibeConfiguration(this IEnumerable<IConfigurationSection> services, ILogger logger)
        {
            services.DescibeConfiguration(x => logger.LogInformation(x));
        }

        public static void DescibeConfiguration(this IConfiguration configuration, Action<string> descriptionSink)
        {
            configuration.GetAllConfigurationSections().DescibeConfiguration(descriptionSink);
        }

        public static void DescibeConfiguration(this IConfiguration configuration, StreamWriter writer)
        {
            configuration.DescibeConfiguration(x => writer.WriteLine(x));
        }

        public static void DescibeConfiguration(this IConfiguration configuration, ILogger logger)
        {
            configuration.DescibeConfiguration(x => logger.LogInformation(x));
        }

        public static IEnumerable<IConfigurationSection> GetAllConfigurationSections(this IConfigurationSection configurationSection)
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
                    var childConfigurationSections = child.GetAllConfigurationSections();
                    foreach (var childConfigurationSection in childConfigurationSections)
                    {
                        yield return childConfigurationSection;
                    }
                }
            }
        }

        public static IEnumerable<IConfigurationSection> GetAllConfigurationSections(this IConfiguration configuration)
        {
            foreach (var configurationSection in configuration.GetChildren())
            {
                foreach (var childConfigurationSection in configurationSection.GetAllConfigurationSections())
                {
                    yield return childConfigurationSection;
                }
            }
        }
    }
}

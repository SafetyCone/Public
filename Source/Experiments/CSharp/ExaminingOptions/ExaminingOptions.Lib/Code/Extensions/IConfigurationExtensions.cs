using System;

using Microsoft.Extensions.Configuration;


namespace ExaminingOptions.Lib
{
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Allows providing either a configuration section or a whole configuration. In the case of a section, the section is just returned. In the case of a whole configuration, a section with the given <paramref name="sectionKey"/> will be returned.
        /// </summary>
        public static IConfigurationSection OrSection(this IConfiguration configuration, string sectionKey)
        {
            var configurationSection = configuration as IConfigurationSection ?? configuration.GetSection(sectionKey);
            return configurationSection;
        }
    }
}

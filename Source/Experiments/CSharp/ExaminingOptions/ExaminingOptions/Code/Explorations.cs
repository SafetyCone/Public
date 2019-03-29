using System;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using ExaminingDependencyInjection.Lib;
using ExaminingOptions.Lib;


namespace ExaminingOptions
{
    public static class Explorations
    {
        public static void SubMain()
        {
            //Explorations.ConsoleWriterOptionsManualConfiguration();
            //Explorations.ConsoleWriterAppSettingsConfiguration();
            //Explorations.ConsoleWriterConfiguratorConfiguration();
            //Explorations.ConsoleWriterTryAddEnumerableConfigurator();
            Explorations.AddDuplicateServices();
            //Explorations.AddDuplicateServicesWithTryAddEnumerable();
        }

        /// <summary>
        /// The TryAddEnumerable() method prevents the same implementation of the same service from being added multiple times.
        /// </summary>
        private static void AddDuplicateServicesWithTryAddEnumerable()
        {
            var services = new ServiceCollection()
                   ;

            services
                   .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterConfigurator>())
                   ;

            services
                   .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterExtraConfigurator>())
                   ;

            services
                   .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterConfigurator>())
                   ;

            services
                   .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterExtraConfigurator>())
                   ;

            var outputFilePath = @"C:\Temp\Services.txt";
            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                services.DescribeServices(streamWriter);
            }
        }

        /// <summary>
        /// There is nothing stopping users from adding the same implementation of the same service multiple times.
        /// </summary>
        private static void AddDuplicateServices()
        {
            var services = new ServiceCollection
            {
                ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterConfigurator>(),

                ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterExtraConfigurator>(),

                ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterConfigurator>(),

                ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterExtraConfigurator>()
            };

            var outputFilePath = @"C:\Temp\Services.txt";
            using (var streamWriter = new StreamWriter(outputFilePath))
            {
                services.DescribeServices(streamWriter);
            }
        }

        /// <summary>
        /// Use multiple other services to configure the <see cref="ConsoleWriterOptions"/>.
        /// </summary>
        private static void ConsoleWriterTryAddEnumerableConfigurator()
        {
            var services = new ServiceCollection()
                   .AddOptions()
                   .AddConsoleWriter()
                   ;

            services
                   .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterConfigurator>())
                   ;

            services
                   .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ConsoleWriterOptions>, ConsoleWriterExtraConfigurator>())
                   ;

            var serviceProvider = services.BuildServiceProvider();

            var consoleWriter = serviceProvider.GetRequiredService<IConsoleWriter>();

            consoleWriter.Write();
        }

        /// <summary>
        /// Use another service to configure the <see cref="ConsoleWriterOptions"/>.
        /// </summary>
        private static void ConsoleWriterConfiguratorConfiguration()
        {
            var services = new ServiceCollection()
                .AddOptions()

                .ConfigureOptions<ConsoleWriterConfigurator>()
                .AddConsoleWriter()
                ;

            var serviceProvider = services.BuildServiceProvider();

            var consoleWriter = serviceProvider.GetRequiredService<IConsoleWriter>();

            consoleWriter.Write();
        }

        /// <summary>
        /// Uses a configuration (built from an appsettings.json file) to configure the <see cref="ConsoleWriterOptions"/>.
        /// </summary>
        private static void ConsoleWriterAppSettingsConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(@"appsettings.json")
                ;

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection()
                .AddOptions()

                //.AddConsoleWriter(configuration.GetSection(@"ConsoleWriter")) // Obsole after OrSection() extension method.
                //.AddConsoleWriter(configuration) // Direct configuration section.
                .AddConsoleWriter(configuration.GetSection(@"App:CustomConsoleWriter")); // Or a configuration sub-section.
                ;

            var serviceProvider = services.BuildServiceProvider();

            var consoleWriter = serviceProvider.GetRequiredService<IConsoleWriter>();

            consoleWriter.Write();
        }

        /// <summary>
        /// Uses a manually code factory method to configure the <see cref="ConsoleWriterOptions"/>.
        /// </summary>
        private static void ConsoleWriterOptionsManualConfiguration()
        {
            var services = new ServiceCollection()
                .AddOptions()

                .AddConsoleWriter(x => x.Message = @"Hello World!") // Manual configuration.
                ;

            var serviceProvider = services.BuildServiceProvider();

            var consoleWriter = serviceProvider.GetRequiredService<IConsoleWriter>();

            consoleWriter.Write();
        }
    }
}

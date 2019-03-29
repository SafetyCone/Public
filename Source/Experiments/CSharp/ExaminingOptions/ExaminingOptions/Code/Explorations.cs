using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ExaminingOptions.Lib;


namespace ExaminingOptions
{
    public static class Explorations
    {
        public static void SubMain()
        {
            //Explorations.ConsoleWriterOptionsManualConfiguration();
            //Explorations.ConsoleWriterAppSettingsConfiguration();
            Explorations.ConsoleWriterConfiguratorConfiguration();
        }

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

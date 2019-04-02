using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.Options;

using ExaminingConfiguration.Lib;
using ExaminingDependencyInjection.Lib;
using ExaminingLogging.Lib;


namespace ExaminingLogging
{
    public class Explorations
    {
        public static void SubMain()
        {
            //Explorations.GetLoggerDirectly();
            //Explorations.GetLoggerFromDI();
            //Explorations.GetLoggerFilterOptions();
            //Explorations.ConfigureAndTestMicrosoftLogging();
            Explorations.GetIntermediateLoggingServices();
        }

        private static void GetIntermediateLoggingServices()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Configurations/Microsoft.Logging.json")

                .Build();

            var configurationDescriptionFilePath = @"C:\Temp\Configuration.txt";
            using (var streamWriter = new StreamWriter(configurationDescriptionFilePath))
            {
                configuration.DescibeConfiguration(streamWriter);
            }

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder
                        .AddConfiguration(loggingBuilder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection(@"Logging"))
                        .AddConsole()
                        .AddDebug()
                        ;
                })
                ;

            var servicesDescriptionFilePath = @"C:\Temp\Services.txt";
            using (var streamWriter = new StreamWriter(servicesDescriptionFilePath))
            {
                services.DescribeServices(streamWriter);
            }

            var serviceProvider = services.BuildServiceProvider();

            var loggingConfigurationAssembly = typeof(ILoggerProviderConfigurationFactory).Assembly;
            var loggingConfigurationType = loggingConfigurationAssembly.GetType(@"Microsoft.Extensions.Logging.Configuration.LoggingConfiguration");
            var enumerableType = typeof(IEnumerable<>);
            var enumerableOfLoggingConfigurationType = enumerableType.MakeGenericType(loggingConfigurationType);
            var loggingConfigurations = serviceProvider.GetRequiredService(enumerableOfLoggingConfigurationType);

            var loggerProviderConfigurationFactory = serviceProvider.GetRequiredService<ILoggerProviderConfigurationFactory>();


            // ConsoleLogger.
            var consoleLoggerProviderType = typeof(ConsoleLoggerProvider);

            var consoleLoggerProviderConfiguration = loggerProviderConfigurationFactory.GetConfiguration(consoleLoggerProviderType); // Somehow this is empty?
            var consoleLoggerConfigurationDescriptionFilePath = @"C:\Temp\Configuration-ConsoleLoggerProvider.txt";
            using (var streamWriter = new StreamWriter(consoleLoggerConfigurationDescriptionFilePath))
            {
                consoleLoggerProviderConfiguration.DescibeConfiguration(streamWriter);
            }

            //var loggerProviderConfigurationGenericType = loggingConfigurationAssembly.GetType(@"Microsoft.Extensions.Logging.Configuration.ILoggerProviderConfiguration`1");
            //var loggerProviderConfigurationOfConsoleType = loggerProviderConfigurationGenericType.MakeGenericType(consoleLoggerProviderType);
            //var loggerProviderConfigurationOfConsole = serviceProvider.GetRequiredService(loggerProviderConfigurationOfConsoleType); // object type, need expression to get configuration.
            var loggerProviderConfigurationOfConsole = serviceProvider.GetRequiredService<ILoggerProviderConfiguration<ConsoleLoggerProvider>>();
            var consoleLoggerConfigurationDescriptionFilePath2 = @"C:\Temp\Configuration-ConsoleLoggerProvider2.txt";
            using (var streamWriter = new StreamWriter(consoleLoggerConfigurationDescriptionFilePath2))
            {
                loggerProviderConfigurationOfConsole.Configuration.DescibeConfiguration(streamWriter);
            }

            var loggerFilterOptionsOptions = serviceProvider.GetRequiredService<IOptions<LoggerFilterOptions>>();
            var loggerFilterOptions = loggerFilterOptionsOptions.Value;

            //Microsoft.Extensions.Logging.Configuration.
        }

        private static void ConfigureAndTestMicrosoftLogging()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(@"Configurations/Microsoft.Logging.json")
                ;

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddLogging(loggingBuilder =>
                {
                    var intermediateServiceProvider = loggingBuilder.Services.BuildServiceProvider();

                    var configurationInstance = intermediateServiceProvider.GetRequiredService<IConfiguration>();

                    loggingBuilder
                        .AddConfiguration(configurationInstance.GetSection(@"Logging"))
                        .AddConsole()
                        .AddDebug()
                        ;
                })
                ;

            var servicesFilePath = @"C:\Temp\Services.txt";
            using (var streamWriter = new StreamWriter(servicesFilePath))
            {
                services.DescribeServices(streamWriter);
            }

            var serviceProvider = services.BuildServiceProvider();

            var explorationsCategoryLogger = serviceProvider.GetRequiredService<ILogger<Explorations>>();

            explorationsCategoryLogger.TestAllLogLevels();

            var microsoftCategoryLogger = serviceProvider.GetLogger(@"Microsoft");

            microsoftCategoryLogger.TestAllLogLevels();

            var microsoftStuffCategoryLogger = serviceProvider.GetLogger(@"Microsoft.Stuff");

            microsoftStuffCategoryLogger.TestAllLogLevels();

            Thread.Sleep(1000); // Required to allow Microsoft loggers to catch up.
        }

        private static void GetLoggerFilterOptions()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            var loggerFilterOptions = serviceProvider.GetRequiredService<IOptions<LoggerFilterOptions>>();
        }

        private static void GetLoggerFromDI()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddProvider(new ExtremelySimpleLoggerProvider());

            var logger = loggerFactory.CreateLogger<Explorations>();

            logger.LogInformation(@"Hello world!", 1, 3.3, DateTime.UtcNow);
        }

        /// <summary>
        /// I want to be able to get a logger directly, without using a service collection or service provider.
        /// This shows how to make a logger factory, add a logger provider, and create a logger.
        /// </summary>
        private static void GetLoggerDirectly()
        {
            var loggerFactory = new LoggerFactory();

            loggerFactory.AddProvider(new ExtremelySimpleLoggerProvider());

            var logger = loggerFactory.CreateLogger<Explorations>();

            logger.LogInformation(@"Hello world!", 1, 3.3, DateTime.UtcNow);
        }
    }
}

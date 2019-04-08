using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ExaminingDependencyInjection.Lib;


namespace ExaminingOptions.Lib
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleWriter(this IServiceCollection services)
        {
            services.AddSingleton<IConsoleWriter, ConsoleWriter>();

            return services;
        }

        /// <summary>
        /// Allow manual configuration.
        /// </summary>
        public static IServiceCollection AddConsoleWriter(this IServiceCollection services, Action<ConsoleWriterOptions> configureOptions)
        {
            services.AddSingleton<IConsoleWriter, ConsoleWriter>();

            services.Configure(configureOptions);

            return services;
        }

        // Obsole after OrSection() extension method.
        //public static IServiceCollection AddConsoleWriter(this IServiceCollection services, IConfigurationSection configurationSection)
        //{
        //    services.AddSingleton<IConsoleWriter, ConsoleWriter>();

        //    services.Configure<ConsoleWriterOptions>(configurationSection);

        //    return services;
        //}

        public static IServiceCollection AddConsoleWriter(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsoleWriter, ConsoleWriter>();

            services.Configure<ConsoleWriterOptions>(configuration.OrSection(@"ConsoleWriter"));

            return services;
        }

        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var output = services.GetIntermediateRequiredService<IConfiguration>();
            return output;
        }

        /// <summary>
        /// Configures <typeparamref name="TOptions"/> using the <see cref="IConfiguration"/> added to the services collection.
        /// </summary>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services)
            where TOptions : class
        {
            var configuration = services.GetConfiguration();

            services
                .Configure<TOptions>(configuration)
                ;

            return services;
        }
    }
}

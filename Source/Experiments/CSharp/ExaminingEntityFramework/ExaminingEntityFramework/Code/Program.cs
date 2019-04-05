using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ExaminingEntityFramework.Lib;


namespace ExaminingEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Program.GetServiceProvider();

            Explorations.SubMain(serviceProvider);
            //Experiments.SubMain(serviceProvider);
            //Demonstrations.SubMain(serviceProvider);
        }

        public static IServiceProvider GetServiceProvider()
        {
            var services = Program.GetServices();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(@"appsettings.json")
                ;

            var configuration = configurationBuilder.Build();
            return configuration;
        }

        public static IServiceCollection GetServices(IConfiguration configuration)
        {
            var connectionStringConfigurationKey = @"Database:ConnectionStrings:TestLocalDatabase";
            var connectionString = configuration[connectionStringConfigurationKey];

            var services = new ServiceCollection()
                .AddOptions()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder
                        .AddConfiguration(configuration.GetSection(@"Logging"))
                        .AddConsole()
                        ;
                })
                .AddDbContext<DatabaseContext>(options =>
                {
                    // Show parameter values.
                    options.EnableSensitiveDataLogging();

                    options.UseSqlServer(connectionString, sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(@"ExaminingEntityFramework.Migrations");
                    });
                })
                ;

            return services;
        }

        public static IServiceCollection GetServices()
        {
            var configuration = Program.GetConfiguration();

            var services = Program.GetServices(configuration);
            return services;
        }
    }
}

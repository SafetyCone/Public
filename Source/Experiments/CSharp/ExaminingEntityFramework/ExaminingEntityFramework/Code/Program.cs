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
            var serviceProvider = Program.BuildServiceProvider();

            Experiments.SubMain(serviceProvider);
            //Demonstrations.SubMain(serviceProvider);
        }

        public static IServiceProvider BuildServiceProvider()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(@"appsettings.json")
                ;

            var configuration = configurationBuilder.Build();

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

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}

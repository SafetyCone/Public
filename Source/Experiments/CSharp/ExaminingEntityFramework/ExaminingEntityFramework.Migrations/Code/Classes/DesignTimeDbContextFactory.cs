using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ExaminingEntityFramework.Lib;


namespace ExaminingEntityFramework.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(@"db.appsettings.json")
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
                    options.UseSqlServer(connectionString, sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(@"ExaminingEntityFramework.Migrations");
                    });
                })
                ;

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.GetRequiredService<DatabaseContext>();
            return result;
        }
    }
}

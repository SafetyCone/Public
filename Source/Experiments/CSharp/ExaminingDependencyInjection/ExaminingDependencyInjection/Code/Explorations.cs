using System;
using System.Linq;
using System.IO;

using Microsoft.Extensions.DependencyInjection;

using ExaminingDependencyInjection.Lib;


namespace ExaminingDependencyInjection
{
    public static class Explorations
    {
        public static void SubMain()
        {
            //Explorations.AddCompoundServiceViaFactoryMethod();
            //Explorations.DetermineOptionsServices();
            Explorations.DetermineLoggingServices();
        }

        private static void DetermineLoggingServices()
        {
            var services = new ServiceCollection();

            // Add options beforehand so we only see the logging services since AddLogging() calls AddOptions().
            services.AddOptions();

            var outputFilePath = @"C:\Temp\Logging services.txt";
            using (var writer = new StreamWriter(outputFilePath))
            {
                Explorations.DetermineNewServices(services, x => x.AddLogging(), writer);
            }
        }

        private static void DetermineOptionsServices()
        {
            var services = new ServiceCollection();

            var outputFilePath = @"C:\Temp\Options Services.txt";
            using (var writer = new StreamWriter(outputFilePath))
            {
                Explorations.DetermineNewServices(services, x => x.AddOptions(), writer);
            }
        }

        private static void DetermineNewServices(IServiceCollection services, Action<IServiceCollection> action, StreamWriter writer)
        {
            var initialServices = services.ToList();

            action(services);

            var afterServices = services.ToList();

            var newServices = afterServices.Except(initialServices);

            newServices.DescribeServices(writer);
        }

        /// <summary>
        /// You CAN use a factory function with a 
        /// </summary>
        private static void UseFactoryFunctionWithGenericService()
        {
            var services = new ServiceCollection()
                //.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>)) // Cannot also specify a factory function.
                .AddSingleton(typeof(IGenericService<>), (serviceProviderInstance) =>
                {
                    return null;
                }) // Cannot also specify a factory function.
                ;

            //var services2 = new ServiceCollection()

            var serviceProvider = services.BuildServiceProvider();

            var compoundService = serviceProvider.GetRequiredService<IGenericService<string>>();
        }

        /// <summary>
        /// Result: No! THIS causes a System.StackOverflowException.
        /// I will frequently want a compound service to be added to a DI container using the same service type as its children. This makes it transparent whether a singular, or compound implementation is being used.
        /// Can I get around the circular reference using a factory method?
        /// </summary>
        private static void AddCompoundServiceViaFactoryMethod()
        {
            var services = new ServiceCollection()
                .AddSingleton<IService, ServiceA>()
                .AddSingleton<IService, ServiceB>()
                .AddSingleton<IService, CompoundService>((serviceProviderInstance) =>
                {
                    var serviceInstances = serviceProviderInstance.GetServices<IService>();

                    var compoundServiceInstance = new CompoundService(serviceInstances);
                    return compoundServiceInstance;
                }) // Compound service is the last implementation type added, so a request for the IService will get an instance of the compound service.
                ;

            //var services2 = new ServiceCollection()

            var serviceProvider = services.BuildServiceProvider();

            var compoundService = serviceProvider.GetRequiredService<IService>();
        }
    }
}

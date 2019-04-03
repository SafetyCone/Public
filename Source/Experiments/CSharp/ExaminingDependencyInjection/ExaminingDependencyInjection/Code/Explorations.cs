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
            //Explorations.DetermineLoggingServices();
            Explorations.IsIServiceScopeFactoryBuiltIn();
        }

        /// <summary>
        /// Result: Somehow, magically, the Microsoft built-in DI container, even though empty, still creates an <see cref="IServiceScopeFactory"/>!
        /// From the .NET Core source (https://github.com/aspnet/Extensions/blob/c19b309cd8839c4e572a7fc10ae873b4c7ee1f93/src/DependencyInjection/DI.Abstractions/src/ServiceProviderServiceExtensions.cs#L125), a call to <see cref="IServiceProvider"/>.CreateScope() simply request a required <see cref="IServiceScopeFactory"/> instance and creates a scope.
        /// Where does this <see cref="IServiceScopeFactory"/> come from? Can any <see cref="ServiceCollection"/> generate it?
        /// </summary>
        private static void IsIServiceScopeFactoryBuiltIn()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            var servicesFilePath = @"C:\Temp\Empty Services.txt";
            using (var streamWriter = new StreamWriter(servicesFilePath))
            {
                services.DescribeServices(streamWriter);
            }
            // Result: services is indeed empty!

            var scope = serviceProvider.CreateScope(); // And yet, an IServiceScopeFactory is available!
        }

        /// <summary>
        /// Result: No, there is no way to use the Microsoft built-in DI container to intercept the scope creation event.
        /// Similar to how singleton service instances can be configured in a custom fashion after instantiation, is there a point at which a service provider will allow scoped service instanes to be configured after instantiation?
        /// Configuring service instances after instantiation is a necessary step since service A could depend on service B, and service B could depend on service A. If the dependency linkage was done in a DI factory method, then an infinite loop (or if the DI container implementation detects cycles, an exception) occurs.
        /// If instead service instances are created, then after both service instances are created, dependency linked, then there is no dependency cycle.
        /// I want to intercept the <see cref="IServiceProvider.cre"/>
        /// </summary>
        private static void ServiceProviderScopeConfiguration()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();

            //serviceProvider.CreateScope();
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

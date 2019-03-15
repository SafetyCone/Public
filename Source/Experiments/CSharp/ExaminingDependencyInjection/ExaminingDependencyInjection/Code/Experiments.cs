using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using ExaminingDependencyInjection.Lib;


namespace ExaminingDependencyInjection
{
    public static class Experiments
    {
        public static void SubMain()
        {
            //Experiments.FindOnlyBySpecifiedServiceType();
            //Experiments.WhichServiceDoIGet();
            Experiments.CanCreateInfiniteLoop();
        }

        /// <summary>
        /// Result: UNEXPECTED. An exception was thrown, not an infinite loop!
        /// What if you have a compound-service of IService whose constructor requires an IEnumerable{IService}, but that is itself an IService?
        /// Expected: An infinite loop occurs.
        /// </summary>
        private static void CanCreateInfiniteLoop()
        {
            var services = new ServiceCollection()
                .AddSingleton<IService, ServiceA>()
                .AddSingleton<IService, ServiceB>()
                .AddSingleton<IService, CompoundService>() // Compound service is the last implementation type added, so a request for the IService will get an instance of the compound service.
                ;

            var serviceProvider = services.BuildServiceProvider();

            var compoundService = serviceProvider.GetRequiredService<IService>(); // Throws exception: System.InvalidOperationException: 'A circular dependency was detected for the service of type ...'
        }

        /// <summary>
        /// Result: Expected. I get an instance of the last service implementation to be added to the DI container.
        /// What happens if I add multiple implementation types for a service type, but then request only one? Do I get the last service type added?
        /// Expected: I should get an instance of the last service type to be added.
        /// </summary>
        private static void WhichServiceDoIGet()
        {
            var writer = Console.Out;

            var services = new ServiceCollection()
                .AddSingleton<IService, ServiceA>()
                .AddSingleton<IService, ServiceB>()
                ;

            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetRequiredService<IService>();

            writer.WriteLine($@"{typeof(IService).Name}: {service.GetType().Name}"); // ServiceB
        }

        /// <summary>
        /// Result: Expected.
        /// Will the DI container find services, that while not added as implmentations of a service type, implement that service type such that they *are* that service type.
        /// Expected: Yes, only if the service type is exactly specified will you get any types. To do otherwise would be over-helpful and break the coherent abstraction.
        /// </summary>
        private static void FindOnlyBySpecifiedServiceType()
        {
            var writer = Console.Out;

            var services = new ServiceCollection()
                .AddSingleton<ServiceA>()
                .AddSingleton<ServiceB>()
                .AddSingleton<CompoundService>()
                ;

            var serviceProvider = services.BuildServiceProvider();

            var compoundService = serviceProvider.GetRequiredService<CompoundService>();

            var servicesCount = compoundService.Services.Count();

            writer.WriteLine($@"Services count: {servicesCount}");
        }
    }
}

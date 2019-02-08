using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using ExaminingClasses.Lib;


namespace ExaminingCSharp
{
    public static class Experiments
    {
        public static void SubMain()
        {
            //TypeExperiments.SubMain();

            //Experiments.CanSetInDictionary();
            //Experiments.ServiceProviderRequiresExactTypeMatch();
            Experiments.ParamsNoValuesIsEmptyOrNull();
        }

        /// <summary>
        /// Result: Expected. Empty.
        /// If a params argument is specified, but no values are provided, is the result empty or null?
        /// Expected: Empty (length of zero).
        /// </summary>
        private static void ParamsNoValuesIsEmptyOrNull()
        {
            void Test(params string[] strs)
            {
                Console.WriteLine(strs);
            }

            Test();
        }

        /// <summary>
        /// Result: True.
        /// For the Microsoft dependency-injection container service provider, will the container do any inheritance-hierarchy traversal to get a service? I.e., if I add a service with type Y: X, then ask for a service of type X, will it give me the service Y? Or do the added and requested services have to match exactly?
        /// The <see cref="ServiceProvider"/> implementation might be willing to try and find a workable service for a request, or it might be picky and demand an exact match between the types of the added and requested service.
        /// Expectation: True, attempting to get a service of a base-type out of a service provider configured to have a service of a derived-type will error.
        /// 
        /// Additionally, there does not seem to be a way to use your own resolver within the service provider. See here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
        /// </summary>
        private static void ServiceProviderRequiresExactTypeMatch()
        {
            var services = new ServiceCollection()
                .AddSingleton<ClassA>()
                ;

            var serviceProvider = services.BuildServiceProvider();

            var interfaceA = serviceProvider.GetRequiredService<IInterfaceA>(); // System.InvalidOperationException: 'No service for type 'ExaminingClasses.Lib.IInterfaceA' has been registered.'
        }

        /// <summary>
        /// Result: False! You CAN set the value of a key that has not yet been added!
        /// Does the System dictionary allow setting values for keys that have not yet been added?
        /// Expected: True, attempting to set the value of a not-yet added key will throw an error.
        /// 
        /// 
        /// </summary>
        private static void CanSetInDictionary()
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var key in dictionary.Keys)
            {
                // Do nothing, this is just to get over the initialization.
            }

            dictionary[@"one"] = 1;
        }
    }
}

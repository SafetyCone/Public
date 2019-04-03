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
            IOExperiments.SubMain();
            //TypeExperiments.SubMain();

            //Experiments.CanSetInDictionary();
            //Experiments.ServiceProviderRequiresExactTypeMatch();
            //Experiments.ParamsNoValuesIsEmptyOrNull();
            //Experiments.DefaultConstructorRunsBeforeInitialization();
            //Experiments.PassInterfaceByReferenceExperiment();
            //Experiments.CanUseNull();
        }

        /// <summary>
        /// Result: UNEXPECTED! The framework seems to successfully handle a null IDisposable instance.
        /// If a method returns an IDisposable, but returns a null instance, does the C# language 'using' construct thrown an exception?
        /// Expected: This will throw an error since an attempt will be made to call NULL.Dispose().
        /// 
        /// The framework only calls Dispose() on non-null objects. See the translation of the 'using' construct shown here: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement.
        /// </summary>
        private static void CanUseNull()
        {
            using (IDisposable disposable = null)
            {
                Console.WriteLine(@"OK");
            }
        }

        /// <summary>
        /// Result: UNEXPECTED! There was no problem completely changing the type of the instance behind an interface when the instance is passed by reference to an interface.
        /// If I pass an interface instance by reference, can I completely replace the instance to which the reference points?
        /// Expected: No. I am not entirely sure about why, perhaps something related to the fundamental need for computers to know the size of all things, but I think something will stop me from re-assigning a reference.
        /// </summary>
        private static void PassInterfaceByReferenceExperiment()
        {
            var interfaceA = new ClassA() as IInterfaceA;

            Console.WriteLine($@"interfaceA type before: {interfaceA.GetType().FullName}");

            Experiments.ReplaceClassAWithClassB(ref interfaceA);

            Console.WriteLine($@"interfaceA type after: {interfaceA.GetType().FullName}");
        }

        private static void ReplaceClassAWithClassB(ref IInterfaceA interfaceA)
        {
            interfaceA = new ClassB();
        }

        /// <summary>
        /// Result: Expected. Yes, the default constructor runs, then values are initialized.
        /// It's possible to initialize objects in C#, and types have default constructors. When an object is initialized, does the default constructor run?
        /// Expected: Yes, the default constructor will run before initializtaion.
        /// </summary>
        private static void DefaultConstructorRunsBeforeInitialization()
        {
            var test = new DefaultConstructorClass()
            {
                Value1 = 3,
            };
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

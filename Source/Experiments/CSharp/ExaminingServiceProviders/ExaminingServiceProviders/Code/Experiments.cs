using System;

using Microsoft.Extensions.DependencyInjection;


namespace ExaminingServiceProviders
{
    public static class Experiments
    {
        public static void SubMain()
        {
            //Experiments.WillServiceProviderProvideItself();
            Experiments.WillServiceProviderProvideAScopeFactory();
        }

        /// <summary>
        /// Result: True.
        /// Use the <see cref="IServiceProvider"/> to get a scope factory, and use scoped services within scopes.
        /// Expectation: True, the service provider is smart enough to provide a scope factory.
        /// 
        /// From a DI container, it's useful to be able to create a scope: https://stackoverflow.com/questions/51939451/how-to-use-a-database-context-in-a-singleton-service
        /// Note that scopes-within-scopes are just another scope (there is no nesting of scopes).
        /// </summary>
        private static void WillServiceProviderProvideAScopeFactory()
        {
            var writer = Console.Out;

            var services = new ServiceCollection()
                .AddScoped<ExampleService>()
                ;

            var serviceProvider = services.BuildServiceProvider();

            var globalExample1 = serviceProvider.GetRequiredService<ExampleService>();
            var globalExample2 = serviceProvider.GetRequiredService<ExampleService>();

            var globalExamplesEqual = Object.ReferenceEquals(globalExample1, globalExample2);
            writer.WriteLine($@"Global service instances are the same instance: {globalExamplesEqual}");

            var scopeFactory1 = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope1 = scopeFactory1.CreateScope())
            {
                var scope1Example1 = scope1.ServiceProvider.GetRequiredService<ExampleService>();
                var scope1Example2 = scope1.ServiceProvider.GetRequiredService<ExampleService>();

                var scope1ExamplesEqual = Object.ReferenceEquals(globalExample1, globalExample2);
                writer.WriteLine($@"Scope1 service instances are the same instance: {scope1ExamplesEqual}");

                var globalEqualsScope1 = Object.ReferenceEquals(globalExample1, scope1Example1);
                writer.WriteLine($@"Global and Scope1 service instances are the same instance: {globalEqualsScope1}");

                var childScopeFactory = scope1.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
                using (var scope2 = childScopeFactory.CreateScope())
                {
                    var scope2Example1 = scope2.ServiceProvider.GetRequiredService<ExampleService>();

                    var globalEqualsScope2 = Object.ReferenceEquals(globalExample1, scope2Example1);
                    writer.WriteLine($@"Global and Scope2 service instances are the same instance: {globalEqualsScope1}");

                    var scope1EqualsScope2 = Object.ReferenceEquals(scope1Example1, scope2Example1);
                    writer.WriteLine($@"Scope1 and Scope2 service instances are the same instance: {globalEqualsScope1}");
                }
            }
        }

        /// <summary>
        /// Result: TRUE! Hallelujah! The <see cref="ServiceProvider"/> is smart enough to give itself upon request.
        /// The <see cref="ServiceProvider"/> class is extremely useful as a dependency-injection container service provider. It handles construction of an instance of any of its registered types.
        /// But will it provide itself?
        /// Expectation: No, the service provider will not just give itself.
        /// 
        /// 
        /// </summary>
        private static void WillServiceProviderProvideItself()
        {
            var services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();

            var serviceProviderAgain = serviceProvider.GetRequiredService<IServiceProvider>();

            var isEqual = Object.ReferenceEquals(serviceProvider, serviceProviderAgain); // False.
            Console.WriteLine($@"Does service provider provide its own exact instance? {isEqual}");
        }
    }
}

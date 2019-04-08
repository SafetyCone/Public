using System;

using Microsoft.Extensions.DependencyInjection;


namespace ExaminingDependencyInjection.Lib
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Build a service provider from the current state of the service collection and get a required service.
        /// </summary>
        public static T GetIntermediateRequiredService<T>(this IServiceCollection services)
        {
            var intermediateServiceProvider = services.BuildServiceProvider();

            var output = intermediateServiceProvider.GetRequiredService<T>();
            return output;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ExaminingDependencyInjection.Lib
{
    public static class ServiceDescriptorExtensions
    {
        public static void DescribeUsing(this ServiceDescriptor service, Action<string> descriptionSink)
        {
            descriptionSink(@"-----");
            descriptionSink($@"Service Type: {service.ServiceType.FullName}");
            descriptionSink($@"Implementation Type: {(service.ImplementationType == null ? @"<null>" : service.ImplementationType.FullName)}");
            descriptionSink($@"{nameof(service.Lifetime)}: {service.Lifetime}");
            descriptionSink($@"Implementation Instance: {(service.ImplementationInstance == null ? @"<null>" : service.ImplementationInstance.ToString())}");
            descriptionSink($@"Implementation Factory: {service.ImplementationFactory}");
        }

        public static void DescribeServices(this IEnumerable<ServiceDescriptor> services, Action<string> descriptionSink)
        {
            foreach (var service in services)
            {
                service.DescribeUsing(descriptionSink);
            }
        }

        public static void DescribeServices(this IEnumerable<ServiceDescriptor> services, StreamWriter writer)
        {
            services.DescribeServices(x => writer.WriteLine(x));
        }

        public static void DescribeServices(this IEnumerable<ServiceDescriptor> services, ILogger logger)
        {
            services.DescribeServices(x => logger.LogInformation(x));
        }
    }
}

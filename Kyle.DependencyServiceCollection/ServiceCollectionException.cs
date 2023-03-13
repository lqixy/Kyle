using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kyle.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.DependencyServiceCollection
{
    public static class ServiceCollectionException
    {
        public static void AddServices(this IServiceCollection services)
        {
            var assemblies = Extensions.AssemblyExtensions.GetAssemblies();
            var lifetimeType = typeof(ISubscribeTransientDependency);
            var types = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => lifetimeType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .ToList();
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    services.AddTransient(@interface, type);
                }
            }
        }

        private static void RegisterSubscribeServices()
        {
            
        }
    }
}

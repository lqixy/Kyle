using Autofac;
using Kyle.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DependencyAutofac
{
    public static class AutofacExtensions
    {
        public static IServiceCollection AddAutofac(this IServiceCollection services, ContainerBuilder builder)
        {
            var assembiles = Extensions.AssemblyExtensions.GetAssemblies();

            builder.RegisterAssemblyTypes(assembiles)
                .Where(x => x.Name.EndsWith("AppService") || x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().SingleInstance()
                ;

            var singletonType = typeof(ISingletonDependency);
            builder
                .RegisterAssemblyTypes(assembiles)
                .Where(x => singletonType.IsAssignableFrom(x) && x != singletonType)
                .AsImplementedInterfaces().SingleInstance();

            var transientType = typeof(ITransientDependency);
            builder
                .RegisterAssemblyTypes(assembiles)
                .Where(x => transientType.IsAssignableFrom(x) && x != transientType)
                .AsImplementedInterfaces().InstancePerDependency();

            return services;
        }

        //public static void AddAutofacModule()
        //{

        //}
    }
}

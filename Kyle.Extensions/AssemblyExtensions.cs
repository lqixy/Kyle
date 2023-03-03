using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions
{
    public static class AssemblyExtensions
    {
        public static Assembly[] GetAssemblies()
        {
            return DependencyContext.Default.CompileLibraries
                .Where(x => !x.Serviceable && x.Type != "package")
                .Select(x => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(x.Name))).ToArray();
        }

        public static bool IsImplement(this Type entityType, Type interfaceType)
        {
            return entityType.GetTypeInfo().GetInterfaces().Any(x =>
                x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }

        public static bool IsSubClassOf(this Type entityType, Type superType)
        {
            return entityType.GetTypeInfo().IsSubClassOf(superType);
        }

    }
}

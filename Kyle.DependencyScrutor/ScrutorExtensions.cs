using Kyle.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DependencyScrutor
{
    public static class ScrutorExtensions
    {
        public static void AddScrutor(this IServiceCollection services)
        {
            services.Scan(selector =>
            {
                selector.FromAssemblies(AssemblyExtensions.GetAssemblies())
                .AddClasses(c => c.Where(x => x.Name.EndsWith("AppService") || x.Name.EndsWith("Repository")))
                .AsImplementedInterfaces().WithSingletonLifetime()
                
                ;
            });
        }
    }
}

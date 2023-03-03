using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.EntityFrameworkExtensions
{
    public static class EntityFrameworkServiceExtensions
    {
        public static void AddEfCore(this IServiceCollection services, IConfiguration configuration)
        {
            //var assembly = Assembly.GetExecutingAssembly();
            //var types = assembly.GetTypes().Where(x => x.IsClass && x.IsAbstract);

            //foreach (var type in types)
            //{
            //    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRepository<>))
            //    {
            //        var interfaceType = typeof(IRepository<>).MakeGenericType(type.GetGenericArguments());
            //        services.AddSingleton(interfaceType, type);
            //    }
            //}
            //services.AddTransient(typeof(IRepository<>), typeof(EfCoreRepositoryBase<>));
            services.AddDbContext<MallDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:Default"]);
            });


        }
    }
}

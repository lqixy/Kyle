using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DapperFrameworkExtensions
{
    public static class DapperServiceExtensions
    {
        public static void AddDapper(this IServiceCollection services)
        {
            //services.Scan(
            //    selector => selector.FromAssemblies(Assembly.GetExecutingAssembly())
            //    .AddClasses()
            //    );
            services.AddSingleton<IConnectionStringResolver, DefaultConnectionStringResolver>()
                .AddSingleton<IDbProviderFactory, SqlServerDbProviderFactory>()
                .AddSingleton<IDatabase, Database>()
                .AddSingleton<SqlDbContext>()
                .AddSingleton<MySqlDbContext>()
                //.AddTransient<IDbContextProvider, DapperDbContextProvider>()                
                //.AddSingleton<DapperDbContextProvider>()
                //.AddTransient(typeof(IDbContextProvider<>), typeof(DapperDbContextProvider<>))
                //.AddTransient(typeof(IDbContextProvider<>))

                ;
        }

        //private static void AddClassesAsImplementedInterface(Type t)
        //{
        //    Extensions.AssemblyExtensions.GetAssemblies()
        //        .Where(x => x.)
        //}
    }
}

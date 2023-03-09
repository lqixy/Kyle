using Autofac;
using Kyle.EntityFrameworkExtensions;
using Kyle.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.TestBase
{
    public class TestBase
    {
        protected IContainer Container;

        public TestBase()
        {
            var assemblies = Extensions.AssemblyExtensions.GetAssemblies();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                ;

            var config = configuration.Build();

            var services = new ServiceCollection();

            services.AddDbContext<MallDbContext>(options =>
            {
                options.UseSqlServer(config["ConnectionStrings:Default"]);
            });

            
            var builder = new ContainerBuilder();

            //builder.RegisterType<MallDbContext>().As(typeof(DbContext)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                           .Where(x => x.Name.EndsWith("AppService") || x.Name.EndsWith("Repository"))
                           .AsImplementedInterfaces().SingleInstance()
                           ;

            var singletonType = typeof(ISingletonDependency);
            builder
                .RegisterAssemblyTypes(assemblies)
                .Where(x => singletonType.IsAssignableFrom(x) && x != singletonType)
                .AsImplementedInterfaces().SingleInstance();

            var transientType = typeof(ITransientDependency);
            builder
                .RegisterAssemblyTypes(assemblies)
                .Where(x => transientType.IsAssignableFrom(x) && x != transientType)
                .AsImplementedInterfaces().InstancePerDependency();

            Container = builder.Build();

        }


    }
}

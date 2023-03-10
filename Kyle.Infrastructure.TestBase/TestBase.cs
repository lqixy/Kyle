using Autofac;
using Kyle.EntityFrameworkExtensions;
using Kyle.Extensions;
using Kyle.Infrastructure.Commanding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                .Build()
                ;

            //EventsExtensions.AddEvents();

            var builder = new ContainerBuilder();

            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();

            //builder.RegisterGeneric(typeof(LocalMessagePublisher)).As(typeof(IMessagePublisher<>))
            //    .InstancePerLifetimeScope();

            builder.RegisterType(typeof(LocalMessagePublisher))
                .As(typeof(IMessagePublisher<IApplicationMessage>))
                .InstancePerLifetimeScope()
                ;
            //builder.RegisterType<LocalMessagePublisher>().As<IMessagePublisher>()
            //    .AsImplementedInterfaces().SingleInstance();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<MallDbContext>();
                optionsBuilder.UseSqlServer(configuration["ConnectionStrings:Default"]);
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<MallDbContext>().AsSelf()
                .InstancePerLifetimeScope();

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

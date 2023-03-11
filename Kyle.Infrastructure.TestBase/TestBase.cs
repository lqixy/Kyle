using Autofac;
using Kyle.DependencyAutofac;
using Kyle.EntityFrameworkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Kyle.Infrastructure.Mediators;

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

            builder.AddAutofac();
            builder.AddMediator();
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<MallDbContext>();
                optionsBuilder.UseSqlServer(configuration["ConnectionStrings:Default"]);
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<MallDbContext>().AsSelf()
                .InstancePerLifetimeScope();

            Container = builder.Build();

        }


    }
}

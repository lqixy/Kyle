using Autofac;
using Kyle.DependencyAutofac;
using Kyle.DependencyServiceCollection;
using Kyle.EntityFrameworkExtensions;
using Kyle.Infrastructure.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Kyle.Infrastructure.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Infrastructure.TestBase
{
    public class TestBase
    {
        protected IContainer Container;

        protected IServiceProvider Provider;

        public TestBase()
        {
            var assemblies = Extensions.AssemblyExtensions.GetAssemblies();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                ;
            var services = new ServiceCollection();
            services.AddDbContext<MallDbContext>();
            services.AddLogging();
            // services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddCAPService(configuration);
            
            services.AddServices();
            //EventsExtensions.AddEvents();
            Provider = services.BuildServiceProvider();
            
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

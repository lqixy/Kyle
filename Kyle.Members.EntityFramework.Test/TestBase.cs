using Autofac;
using Kyle.DependencyAutofac;
using Kyle.EntityFrameworkExtensions;
using Kyle.Extensions;
using Kyle.Members.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.EntityFramework.Test
{
    public class TestBase
    {
        protected ServiceProvider provider;

        public TestBase()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            ;

            var services = new ServiceCollection();
            //services.AddSingleton<IUserQueryRepository, UserQueryRepository>();
            AutoFac();
            services.AddEfCore(configuration);


            provider = services.BuildServiceProvider();
        }

        private void AutoFac()
        {
            var builder = new ContainerBuilder();
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
        }
    }
}

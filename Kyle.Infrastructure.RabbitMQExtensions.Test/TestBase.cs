using Autofac;
using Kyle.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions.Test
{
    public class TestBase
    {
        protected IContainer Container;

        public TestBase()
        {
            //var services = new ServiceCollection()
            //    .AddLogging()
            //    ;

            EventsExtensions.AddEvents();

            var builder = new ContainerBuilder();
            builder.RegisterType<NullLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            RabbitMQServiceExtensions.AddRabbitMQ(builder);

            Container = builder.Build();

        }
    }
}

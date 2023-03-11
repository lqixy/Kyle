using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

            // EventsExtensions.AddEvents();

            var builder = new ContainerBuilder();
            builder.RegisterType<NullLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            RabbitMQServiceExtensions.AddRabbitMQ(builder);

            Container = builder.Build();

        }
    }
}

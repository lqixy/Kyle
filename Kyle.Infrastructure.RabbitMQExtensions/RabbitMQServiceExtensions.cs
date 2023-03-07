using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions
{
    public static class RabbitMQServiceExtensions
    {
        public static void AddRabbitMQ(ContainerBuilder builder)
        {

            builder.RegisterType<RabbitMQMessageSerializer>().SingleInstance();
            builder.RegisterType<RabbitMQOptions>().SingleInstance();
            //builder.RegisterType<ApplicationMessage>().As<IApplicationMessage>();
            builder.RegisterType<ConnectionPool>().As<IConnectionPool>().AsImplementedInterfaces().SingleInstance();

            var assemblies = Extensions.AssemblyExtensions.GetAssemblies();

            builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IMessagePublisher<>))
                .AsImplementedInterfaces();


            //builder.RegisterType<ApplicationMessagePublisher>().As<IMessagePublisher<IApplicationMessage>>().SingleInstance();

        }
    }
}

using Autofac;
using Kyle.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure
{
    public static class EventsExtensions
    {
        public static void AddEvents()
        {
            //var bus = new EventBus();

            //var assemblies = Kyle.Extensions.AssemblyExtensions.GetAssemblies();
            //var types = assemblies.SelectMany(x => x.GetTypes());

            //bus.Builder.RegisterAssemblyTypes(assemblies)
            //    .AsClosedTypesOf(typeof(IEventHandler<>))
            //    .AsImplementedInterfaces();

            //bus.Container = bus.Builder.Build();

            //var handlers = bus.Container.Resolve<IEnumerable<IEventHandler>>();
            //foreach (var handler in handlers)
            //{
            //    var interfaces = handler.GetType().GetInterfaces();
            //    foreach (var @interface in interfaces)
            //    {
            //        if (!typeof(IEventHandler).IsAssignableFrom(@interface)) continue;

            //        var genericArgs = @interface.GetGenericArguments();
            //        if (genericArgs.Length == 1)
            //        {
            //            bus.Register(genericArgs[0], handler.GetType());
            //        }
            //    }
            //}


            //return bus;
        }
    }
}

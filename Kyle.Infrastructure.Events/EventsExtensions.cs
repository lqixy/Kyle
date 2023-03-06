using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Kyle.Infrastructure.Events.Bus;
using Kyle.Infrastructure.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events
{
    public static class EventsExtensions
    {
        public static void AddEventService()
        {
            var bus = new EventBus();

            var types = Kyle.Extensions.AssemblyExtensions.GetAssemblies()
                .SelectMany(x => x.GetTypes());
            ;

            bus.Container.Register(Classes.From(types)
                 .BasedOn(typeof(IEventHandler<>))
                 .WithService.Base()
                 );

            var handlers = bus.Container.Kernel.GetAssignableHandlers(typeof(IEventHandler));
            foreach (var handler in handlers)
            {
                var interfaces = handler.ComponentModel.Implementation.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).IsAssignableFrom(@interface)) continue;

                    var genericArgs = @interface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        bus.Register(genericArgs[0], handler.ComponentModel.Implementation);
                    }
                }
            }
        }
    }
}

using Kyle.Infrastructure.Event.Handlers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Event.Bus
{
    internal class EventTypeWithEventHandlerFactories
    {
        public Type EventType { get;}

        public List<IEventHandlerFactory> EventHandlerFactories { get;}

        public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
        {
            EventType = eventType;
            EventHandlerFactories = eventHandlerFactories;
        }
    }
}

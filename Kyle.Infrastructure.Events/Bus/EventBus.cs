using Autofac;
using Autofac.Builder;
//using Castle.MicroKernel.Registration;
//using Castle.Windsor;
using Kyle.Infrastructure.Events.Handlers;
using Kyle.Infrastructure.Events.Stores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Bus
{
    public class EventBus : IEventBus
    {
        private readonly IEventStore _store;

        public ContainerBuilder Builder { get; private set; }

        //public IWindsorContainer WindsorContainer { get; private set; }
        public IContainer Container { get; set; }

        public static EventBus Default { get; private set; }

        public EventBus()
        {
            _store = new EventStore();
            //WindsorContainer = new WindsorContainer();

            Builder = new ContainerBuilder();
        }

        static EventBus()
        {
            Default = new EventBus();
        }

        public void Register<TEventData>(IEventHandler eventHandler) where TEventData : IEventData
        {
            Register(typeof(TEventData), eventHandler.GetType());
        }

        public void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            var actionHandler = new ActionEventHandler<TEventData>(action);

            Builder.RegisterType(actionHandler.GetType()).As<ActionEventHandler<TEventData>>();

            //WindsorContainer.Register(Component.For<IEventHandler<TEventData>>()
            //    .UsingFactoryMethod(() => actionHandler)
            //    )
            //    ;

            Register<TEventData>(actionHandler);
        }

        public void Register(Type eventType, Type handlerType)
        {
            var handlerInterface = handlerType.GetInterface("IEventHandler`1");
            if (!Container.IsRegistered(handlerInterface))
            {
                //Builder.Register();
                Builder.RegisterType(handlerType).As(handlerInterface);
            }

            //if (!WindsorContainer.Kernel.HasComponent(handlerInterface))
            //{
            //    WindsorContainer.Register(Component.For(handlerInterface, handlerType));
            //}

            _store.AddRegister(eventType, handlerType);
        }

        public void RegisterAllEventHandlerFromAssembly(Assembly assembly)
        {
            //throw new NotImplementedException();
        }

        public void UnRegister<TEventData>(Type handlerType) where TEventData : IEventData
        {
            _store.RemoveRegister(typeof(TEventData), handlerType);
        }

        public void UnRegisterAll<TEventData>() where TEventData : IEventData
        {
            var handlerTypes = _store.GetHandlersForEvent(typeof(TEventData)).ToList();
            foreach (var handlerType in handlerTypes)
            {
                _store.RemoveRegister(typeof(TEventData), handlerType);
            }
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            var handlerTypes = _store.GetHandlersForEvent(eventData.GetType()).ToList();
            if (handlerTypes.Any())
            {
                foreach (var handlerType in handlerTypes)
                {
                    var handlerInterface = handlerType.GetInterface("IEventHandler`1");
                    //var eventHandlers = WindsorContainer.ResolveAll(handlerInterface);
                    var eventHandlers = Container.Resolve<IEnumerable<IEventHandler<TEventData>>>();

                    foreach (var eventHandler in eventHandlers)
                    {
                        if (eventHandler.GetType() == handlerType)
                        {
                            var handler = eventHandler as IEventHandler<TEventData>;
                            handler?.HandleEvent(eventData);
                        }
                    }
                }
            }
        }

        public void Trigger<TEventData>(Type eventHandlerType, TEventData eventData) where TEventData : IEventData
        {
            if (_store.HasRegisterForEvent<TEventData>())
            {
                var handlers = _store.GetHandlersForEvent<TEventData>();
                if (handlers.Any(x => x == eventHandlerType))
                {
                    //var handlerInterface = eventHandlerType.GetInterface("IEventHandler`1");

                    //var eventHandlers = WindsorContainer.ResolveAll(handlerInterface);
                    var eventHandlers = Container.Resolve<IEnumerable<IEventHandler<TEventData>>>();
                    foreach (var eventHandler in eventHandlers)
                    {
                        if (eventHandler.GetType() == eventHandlerType)
                        {
                            var handler = eventHandler as IEventHandler<TEventData>;
                            handler?.HandleEvent(eventData);
                        }
                    }
                }
            }
        }

        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return Task.Run(() => Trigger(eventData));
        }

        public Task TriggerAsycn<TEventData>(Type eventHandlerType, TEventData eventData) where TEventData : IEventData
        {
            return Task.Run(() => Trigger(eventHandlerType, eventData));
        }

        private readonly ConcurrentDictionary<string, Type> _dic = new ConcurrentDictionary<string, Type>();

        public Type GetType(string name)
        {
            Type t;
            if (_dic.TryGetValue(name, out t)) return t;

            foreach (var item in _store._mapping.Keys)
            {
                var typeName = GetTypeName(item);
                var type = string.Compare(name, typeName) == 0;
                if (type)
                {
                    _dic.TryAdd(typeName, item);
                    return item;
                }
            }
            return null;
        }

        public string GetTypeName(Type type)
        {
            return type.FullName;
        }
    }
}

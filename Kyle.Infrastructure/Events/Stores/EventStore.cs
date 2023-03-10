using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events.Stores
{
    public class EventStore : IEventStore
    {
        private static readonly object LockObj = new object();

        private ConcurrentDictionary<Type, List<Type>> mapping;

        public EventStore()
        {
            mapping ??= new ConcurrentDictionary<Type, List<Type>>();
        }

        public ConcurrentDictionary<Type, List<Type>> _mapping { get { return mapping; } set { } }
        public void AddActionRegister<T>(Action<T> action) where T : IEventData
        {
            var actionHandler = new ActionEventHandler<T>(action);
            AddRegister(typeof(T), actionHandler.GetType());
        }

        public void AddRegister<T, TH>()
            where T : IEventData
            where TH : IEventHandler
        {
            var handlerToRemove = FindRegisterToRemove(typeof(T), typeof(TH));
            RemoveRegister(typeof(T), handlerToRemove);
        }

        private Type FindRegisterToRemove(Type eventData, Type eventHandler)
        {
            if (!HasRegisterForEvent(eventData)) return null;

            return _mapping[eventData].FirstOrDefault(x => x == eventHandler);
        }

        public void AddRegister(Type eventData, Type eventHandler)
        {
            lock (LockObj)
            {
                if (!HasRegisterForEvent(eventData))
                {
                    var handlers = new List<Type>();
                    _mapping.TryAdd(eventData, handlers);
                }

                if (_mapping[eventData].All(x => x != eventHandler))
                {
                    _mapping[eventData].Add(eventHandler);
                }
            }
        }

        public void Clear()
        {
            _mapping.Clear();
        }

        public Type GetEventTypeByName(string eventName)
        {
            return _mapping.Keys.FirstOrDefault(x => x.Name == eventName);
        }

        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData
        {
            return GetHandlersForEvent(typeof(T));
        }

        public IEnumerable<Type> GetHandlersForEvent(Type eventData)
        {
            if (HasRegisterForEvent(eventData)) return _mapping[eventData];

            return new List<Type>();
        }

        public bool HasRegisterForEvent<T>() where T : IEventData
        {
            return _mapping.ContainsKey(typeof(T));
        }

        public bool HasRegisterForEvent(Type eventData)
        {
            return _mapping.ContainsKey(eventData);
        }

        public void RemoveActionRegister<T>(Action<T> action) where T : IEventData
        {
            var actionHandler = new ActionEventHandler<T>(action);
            var handlerToRemove = FindRegisterToRemove(typeof(T), actionHandler.GetType());
            RemoveRegister(typeof(T), handlerToRemove);
        }

        public void RemoveRegister<T, TH>()
            where T : IEventData
            where TH : IEventHandler
        {
            var handlerToRemove = FindRegisterToRemove(typeof(T), typeof(TH));
            RemoveRegister(typeof(T), handlerToRemove);
        }

        public void RemoveRegister(Type eventData, Type eventHandler)
        {
            if (eventHandler != null)
            {
                lock (LockObj)
                {
                    _mapping[eventData].Remove(eventHandler);
                    if (!_mapping[eventData].Any())
                    {
                        List<Type> removedHandlers;
                        _mapping.TryRemove(eventData, out removedHandlers);
                    }
                }
            }
        }
    }
}

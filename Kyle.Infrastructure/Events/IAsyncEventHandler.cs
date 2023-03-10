using Kyle.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events
{
    public interface IAsyncEventHandler<in TEventData> : IEventHandler
    {
        Task HandleEventAsync(TEventData eventData);
    }

    public class AsyncActionEventHandler<TEventData> : IAsyncEventHandler<TEventData>
        , ITransientDependency
    {
        public Func<TEventData, Task> Action { get; private set; }

        public AsyncActionEventHandler(Func<TEventData, Task> action)
        {
            Action = action;
        }

        public async Task HandleEventAsync(TEventData eventData)
        {
            await Action(eventData);
        }
    }
}

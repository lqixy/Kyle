using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Event.Handlers
{
    internal class AsyncActionEventHandler<TEventData> : IAsyncEventHandler<TEventData>
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

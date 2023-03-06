using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Event.Handlers
{
    public class ActionEventHandler<TEventData> : IEventHandler<TEventData> where TEventData : IEventData
    {
        public Action<TEventData> Action { get; set; }

        public ActionEventHandler(Action<TEventData> action)
        {
            Action = action;
        }

        public void HandleEvent(TEventData eventData)
        {
            Action(eventData);
        }
    }
}

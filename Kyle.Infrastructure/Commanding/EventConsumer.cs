using Kyle.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    public class EventConsumer : DisposableObject, IEventConsumer
    {
        private readonly IMessageSubscriber subscriber;

        public IMessageSubscriber Subscriber => subscriber;

        public EventConsumer(IMessageSubscriber subscriber,IEventBus eventBus)
        {
            this.subscriber= subscriber;

            subscriber.MessageReceived += (sender, e) =>
            {
                var type = eventBus.GetType(e.Message.CommandName);
                if(type != null)
                {
                    eventBus.Trigger(type, (IEventData)e.Message.MessageDataBody.ToObject(type));
                }
            };
        }
    }
}

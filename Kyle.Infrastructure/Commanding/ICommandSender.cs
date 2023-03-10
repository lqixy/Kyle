using Kyle.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    public interface ICommandSender : IMessagePublisher
    {
    }

    public class LocalCommandSender : ICommandSender
    {
        public IEventBus EventBus { get; set; } = new EventBus();

        public void Dispose() { }

        public void PublishMessage<TMessage>(TMessage message)
        {
            var type = EventBus.GetType((message as MessageData).CommandName);
            EventBus.Trigger(type, (IEventData)(message as MessageData).MessageDataBody.ToObject(type));
        }
    }
}

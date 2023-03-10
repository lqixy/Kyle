using Kyle.Infrastructure.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    public interface IMessagePublisher<TMessage> : IDisposable where TMessage :class, IMessage
    {
        Task<AsyncTaskResult> PublishMessage(TMessage message);

        AsyncTaskResult Publish(TMessage message);
    }

    public interface IMessagePublisher : IDisposable
    {
        void PublishMessage<TMessage>(TMessage message);
    }

    public class LocalMessagePublisher : IMessagePublisher<IApplicationMessage>
    {
        public ILogger Logger { get; set; }

        public IEventBus EventBus { get; set; } = new EventBus();

        //public LocalMessagePublisher(ILoggerFactory logger)
        //{
        //    Logger = logger.CreateLogger<LocalMessagePublisher>();
        //}

        public void Dispose()
        {

        }

        public AsyncTaskResult Publish(IApplicationMessage message)
        {
            var type = EventBus.GetType(message.GetTypeName());
            if (type == null)
            {
                //Logger.LogWarning($"Not found EventHandler<{message.GetTypeName()}>");
                return AsyncTaskResult.Success;
            }
            EventBus.Trigger(type, message as IEventData);
            return AsyncTaskResult.Success;
        }

        public async Task<AsyncTaskResult> PublishMessage(IApplicationMessage message)
        {
            var type = EventBus.GetType(message.GetTypeName());
            if (type == null)
            {
                //Logger.LogWarning($"Not found EventHandler<{message.GetTypeName()}>");
                return AsyncTaskResult.Success;
            }

            await EventBus.TriggerAsycn(type, message as IEventData);
            return AsyncTaskResult.Success;
        }
    }
}

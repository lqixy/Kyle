using Kyle.Infrastructure.RabbitMQExtensions.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions
{
    public interface IMessagePublisher<TMessage> : IDisposable where TMessage : class, IApplicationMessage
    {
        Task<AsyncTaskResult> PublishAsync(TMessage message);

        AsyncTaskResult Publish(TMessage message);
    }
}

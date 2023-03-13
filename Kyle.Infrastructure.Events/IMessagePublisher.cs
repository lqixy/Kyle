using DotNetCore.CAP;
using Kyle.Infrastructure.CAP;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kyle.Infrastructure.Events;

public interface IMessagePublisher
{
    Task Publish<TMessage>(TMessage message) where TMessage : IEventData;
}

public class MessagePublisher : IMessagePublisher
{
    private readonly ICapPublisher _publisher;
    private readonly ILogger _logger;

    public MessagePublisher(ICapPublisher publisher
        , ILogger<MessagePublisher> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Publish<TMessage>(TMessage message) where TMessage : IEventData
    {
        var data = JsonConvert.SerializeObject(message);
        _logger.LogInformation($"[x] Sent {data}");
        await _publisher.PublishAsync(CAPMessageNamedConst.MESSAGE_NAME, message);
    }
}
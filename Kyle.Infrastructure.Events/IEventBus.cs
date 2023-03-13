using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kyle.Infrastructure.Events;

public interface IEventBus
{
    Task Send<TEventData>(TEventData eventData) where TEventData : IEventData;
}

public class EventBus : IEventBus
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public EventBus(ILogger<EventBus> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Send<TEventData>(TEventData eventData) where TEventData : IEventData
    {
        var message = JsonConvert.SerializeObject(eventData);
        _logger.LogInformation($"[x] Sent: {message}");
        await _mediator.Publish(eventData);
    }
}
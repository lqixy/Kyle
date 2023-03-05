namespace Kyle.Infrastructure.Event.Bus.Handlers;

public interface IEventHandler
{
    
}

public interface IEventHandler<in TEventData> : IEventHandler
{
    /// <summary>
    /// 操作工
    /// </summary>
    /// <param name="eventData"></param>
    void HandleEvent(TEventData eventData);
}

public interface IAsyncEventHandler<in TEventData> : IEventHandler
{
    Task HandlerEventAsync(TEventData eventData);
}
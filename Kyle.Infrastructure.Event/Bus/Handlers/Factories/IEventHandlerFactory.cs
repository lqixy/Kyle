namespace Kyle.Infrastructure.Event.Bus.Handlers.Factories;

public interface IEventHandlerFactory
{
    IEventHandler GetHandler();

    Type GetHandlerType();

    void ReleaseHandler(IEventHandler handler);
}

internal class SingleInstanceHandlerFactory : IEventHandlerFactory
{
    public IEventHandler HandlerInstance { get; private set; }

    public SingleInstanceHandlerFactory(IEventHandler handler)
    {
        HandlerInstance = handler;
    }

    public IEventHandler GetHandler()
    {
        return HandlerInstance;
    }

    public Type GetHandlerType()
    {
        return HandlerInstance.GetType();
    }

    public void ReleaseHandler(IEventHandler handler)
    {
        
    }
}
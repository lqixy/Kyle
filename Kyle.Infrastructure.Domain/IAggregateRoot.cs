using Kyle.Infrastructure.Event.Bus;

namespace Kyle.Infrastructure.Domain;

public interface IAggregateRoot:IGeneratesDomainEvents
{
    string AggregateRootId { get; }
    int Version { get; }
}

public interface IAggregateRoot<TKey> : IEntity<TKey>,IGeneratesDomainEvents
{
    
}

public interface IGeneratesDomainEvents
{
    Queue<IEventData> DomainEvents { get; }
}
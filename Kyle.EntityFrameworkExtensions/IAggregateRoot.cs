using Kyle.EntityFrameworkExtensions.Events;
using Kyle.Infrastructure.Events;
using MediatR;

namespace Kyle.EntityFrameworkExtensions;

public interface IAggregateRoot: IGeneratesDomainEvents
{
    // string AggregateRootId { get; }
    
    int Version { get; }
}

public interface IGeneratesDomainEvents
{
    Queue<IEventData> DomainEvents { get; }
}
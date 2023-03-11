using MediatR;

namespace Kyle.EntityFrameworkExtensions;

public interface IAggregateRoot: IGeneratesDomainEvents
{
    // string AggregateRootId { get; }
    
    int Version { get; }
}

public interface IGeneratesDomainEvents
{
    Queue<IRequest> DomainEvents { get; }
}
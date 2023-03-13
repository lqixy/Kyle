using System.ComponentModel.DataAnnotations.Schema;
using Kyle.EntityFrameworkExtensions.Events;
using Kyle.Infrastructure.Events;
using MediatR;

namespace Kyle.EntityFrameworkExtensions;

public abstract class AggregateRoot: IAggregateRoot
{
    // [NotMapped]
    // public abstract  string AggregateRootId { get; set; }
    //
    [NotMapped]
    public virtual Queue<IEventData> DomainEvents { get; set; }
    
    public int Version { get; set; }

    public AggregateRoot()
    {
        DomainEvents = new Queue<IEventData>();
    }

    public void ApplyEvent(IEventData data)
    {
        DomainEvents.Enqueue(data);
    }
}

// public interface IEventData : IRequest
// {
//     
// }

public abstract class AggregateRoot<TKey> : AggregateRoot
{
    
}


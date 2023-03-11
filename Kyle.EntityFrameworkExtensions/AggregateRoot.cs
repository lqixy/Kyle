using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace Kyle.EntityFrameworkExtensions;

public abstract class AggregateRoot: IAggregateRoot
{
    // [NotMapped]
    // public abstract  string AggregateRootId { get; set; }
    //
    [NotMapped]
    public virtual Queue<IRequest> DomainEvents { get; set; }
    
    public int Version { get; set; }

    public AggregateRoot()
    {
        DomainEvents = new Queue<IRequest>();
    }

    public void ApplyEvent(IRequest data)
    {
        DomainEvents.Enqueue(data);
    }
}

public abstract class AggregateRoot<TKey> : AggregateRoot
{
    
}


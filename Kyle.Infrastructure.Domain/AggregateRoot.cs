using Kyle.Infrastructure.Events;

namespace Kyle.Infrastructure.Domain;

public class AggregateRoot : AggregateRoot<Guid>,IAggregateRoot<Guid>
{
    
}

public class AggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot<TKey>
{
    protected TKey _id;

    public TKey AggregateRootId
    {
        get { return _id; }
        set { _id = value; }
    }
    public virtual Queue<IEventData> DomainEvents { get; }

    public AggregateRoot()
    {
        DomainEvents = new Queue<IEventData>();
    }

    public void ApplyEvent(IEventData data)
    {
        if (data == null) throw new ArgumentException("eventData");

        //data.AggregateRootId = $"{AggregateRootId}";
        DomainEvents.Enqueue(data);
    }
}
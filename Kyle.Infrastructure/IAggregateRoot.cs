using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure
{
    public interface IAggregateRoot : IGeneratesDomainEvents
    {
        string AggregateRootId { get; }
        int Version { get; }
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>, IGeneratesDomainEvents
    {

    }

    public interface IGeneratesDomainEvents
    {
        Queue<IEventData> DomainEvents { get; }
    }
}

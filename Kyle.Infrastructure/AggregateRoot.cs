using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure
{

    public abstract class AggregateRoot : IAggregateRoot
    {
        [NotMapped]
        public abstract string AggregateRootId { get; }

        [NotMapped]
        public virtual Queue<IEventData> DomainEvents { get; }


        public int Version { get; set; }

        public AggregateRoot()
        {
            DomainEvents = new Queue<IEventData>();
        }

        public void ApplyEvent(IEventData data)
        {
            if (data == null) throw new ArgumentException("eventData");

            Version++;

            //data.AggregateRootId = $"{AggregateRootId}";
            DomainEvents.Enqueue(data);
        }
    }
    public abstract class AggregateRoot<TKey> : AggregateRoot, IEntity<TKey> //, IAggregateRoot<TKey>
    {
        //[NotMapped]
        //public abstract string AggregateRootId { get; }

        //[NotMapped]
        //public virtual Queue<IEventData> DomainEvents { get; }

        //public int Version { get; set; }




    }
}

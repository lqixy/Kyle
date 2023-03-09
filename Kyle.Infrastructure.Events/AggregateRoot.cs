using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Events
{
    public abstract class AggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot<TKey>
    {
        //protected TKey _id;

        //public TKey AggregateRootId
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

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
}

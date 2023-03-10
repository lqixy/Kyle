using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Commanding
{
    [Serializable]
    public abstract class ApplicationMessage : IEventData, IMessage
    {
        protected ApplicationMessage() { }

        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Sequence { get; set; }

        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        public bool IsPublisher { get; set; } = true;

        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        public virtual string GetRoutingKey()
        {
            return null;
        }

        public virtual string GetTypeName()
        {
            return this.GetType().FullName;
        }


    }
}

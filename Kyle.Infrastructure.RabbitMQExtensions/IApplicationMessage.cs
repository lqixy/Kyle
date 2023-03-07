using Kyle.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RabbitMQExtensions
{
    public interface IApplicationMessage
    {
        string Id { get; set; }

        string GetRoutingKey();

        string GetTypeName();
    }

    [Serializable]
    public abstract class ApplicationMessage : IEventData, IApplicationMessage
    {
        public ApplicationMessage()
        {
            EventTime = DateTime.Now;
        }

        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        public virtual string GetRoutingKey()
        {
            return null;
        }

        public string GetTypeName()
        {
            return this.GetType().FullName;
        }
    }
}

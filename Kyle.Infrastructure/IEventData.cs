using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure
{
    public interface IEventData
    {
        DateTime EventTime { get; set; }

        object EventSource { get; set; }

        bool IsPublisher { get; set; }

        string AggregateRootId { get; set; }

        int Version { get; set; }
    }

    [Serializable]
    public abstract class EventData : IEventData
    {
        public DateTime EventTime { get; set; }
        public object EventSource { get; set; }
        public bool IsPublisher { get; set; } = false;
        public string AggregateRootId { get; set; }
        public int Version { get; set; }

    }
}

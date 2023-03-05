namespace Kyle.Infrastructure.Event.Bus;

[Serializable]
public abstract class EventData : IEventData
{
    public DateTime EventTime { get; set; }

    public object EventSource { get; set; }

    public bool IsPublisher { get; set; } = false;

    public string AggregateRootId { get; set; }

    public int Version { get; set; }

    protected EventData()
    {
        EventTime = DateTime.Now;
    }
}


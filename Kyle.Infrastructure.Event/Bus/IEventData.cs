namespace Kyle.Infrastructure.Event.Bus;

public interface IEventData
{
    DateTime EventTime { get; set; }

    object EventSource { get; set; }

    bool IsPublisher { get; set; }

    string AggregateRootId { get; set; }
    int Version { get; set; }
}
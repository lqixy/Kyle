namespace Kyle.Infrastructure.CAP.Test;

public class PublisherData
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreationTime { get; set; } = DateTime.Now;
}
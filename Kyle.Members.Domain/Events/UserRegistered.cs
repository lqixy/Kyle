using Kyle.Infrastructure.Events;

namespace Kyle.Members.Domain.Events;

public class UserRegistered: EventData
{
    public Guid UserId { get; set; }

    public Guid TenantId { get; set; }

    public UserRegistered(Guid userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }

    public UserRegistered()
    {
    }
}
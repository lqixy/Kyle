using MediatR;

namespace Kyle.Members.Domain.Events;

public class UserRegisteredHaveRecord: IRequest
{
    public Guid UserId { get; set; }

    public Guid TenantId { get; set; }

    public UserRegisteredHaveRecord(Guid userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }

    public UserRegisteredHaveRecord()
    {
    }
}
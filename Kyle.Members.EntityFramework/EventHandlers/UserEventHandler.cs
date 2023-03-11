using Kyle.Members.Domain;
using Kyle.Members.Domain.Events;
using MediatR;

namespace Kyle.Members.EntityFramework.EventHandlers;

public class UserEventHandler: IRequestHandler<UserRegisteredHaveRecord>
{
    private readonly IUserRegisterRecordRepository _repository;

    public UserEventHandler(IUserRegisterRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UserRegisteredHaveRecord request, CancellationToken cancellationToken)
    {
        await _repository.Insert(new UserRegisterRecord(request.UserId,request.TenantId));
    }
}
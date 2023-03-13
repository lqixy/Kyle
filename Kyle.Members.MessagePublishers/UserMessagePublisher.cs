using DotNetCore.CAP;
using Kyle.Infrastructure.Events;
using Kyle.Members.Domain.Events;
using Kyle.Members.Messages;
using MediatR;

namespace Kyle.Members.MessagePublishers;

public class UserMessagePublisher: INotificationHandler<UserRegistered>
{
    // public readonly ICapPublisher _publisher;
    private readonly IMessagePublisher _publisher;

    public UserMessagePublisher(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var message = new UserRegisteredMessage(10,1,"注册账号送积分");
        // await _publisher.PublishAsync("Q-Test",message);
        await _publisher.Publish(message);
    }
}
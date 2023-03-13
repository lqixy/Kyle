using Kyle.Infrastructure.Events;

namespace Kyle.Members.Messages;

public class UserRegisteredMessage: ApplicationMessage
{
    public UserRegisteredMessage()
    {
    }

    public UserRegisteredMessage(int score, byte type, string reason)
    {
        Score = score;
        Type = type;
        Reason = reason;
    }

    public int Score { get; set; }

    public byte Type { get; set; }

    public string Reason { get; set; }
}
using RabbitMQ.Client;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public class RabbitMqConnections : Dictionary<string, ConnectionFactory>
{
    public const string DefaultConnectionName = "Default";

    public ConnectionFactory Default
    {
        get => this[DefaultConnectionName];
        set => this[DefaultConnectionName] = value;
    }

    public RabbitMqConnections()
    {
        Default = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "kyle",
            Password = "19841230",
            VirtualHost = "mall"
        };
    }

    public ConnectionFactory GetOrDefault(string connectionName)
    {
        if (TryGetValue(connectionName, out var connectionFactory))
        {
            return connectionFactory;
        }

        return Default;
    }

}
using System.Collections.Concurrent;
using RabbitMQ.Client;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public interface IConnectionPool
{
    IConnection Get(string connectionName = null);
}

public class ConnectionPool : IConnectionPool
{
    protected RabbitMQOptions Options { get; }
    protected ConcurrentDictionary<string, Lazy<IConnection>> Connections { get; }

    private bool _isDisposed;

    public ConnectionPool(RabbitMQOptions options)
    {
        Options = options;
        Connections = new ConcurrentDictionary<string, Lazy<IConnection>>();
    }


    public virtual IConnection Get(string connectionName = null)
    {
        connectionName = connectionName ?? RabbitMqConnections.DefaultConnectionName;

        return Connections.GetOrAdd(connectionName,
            (x) => { return new Lazy<IConnection>(() => Options.Connections.GetOrDefault(x).CreateConnection()); }).Value;
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        foreach (var connection in Connections.Values)
        {
            try
            {
                connection.Value.Dispose();
            }
            catch
            {
            }

            Connections.Clear();
        }
    }
}
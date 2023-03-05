using System.Collections.Concurrent;
using RabbitMQ.Client;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public interface IConnectionPool
{
    IConnection Get(string connectionName = null);
}

public class ConnectionPool : IConnectionPool
{
    protected ConcurrentDictionary<string, Lazy<IConnection>> Connections { get; }

    private bool _isDisposed;

    public ConnectionPool()
    {
        Connections = new ConcurrentDictionary<string, Lazy<IConnection>>();
    }


    public virtual IConnection Get(string connectionName = null)
    {
        connectionName = connectionName ?? "Default";

        return Connections.GetOrAdd(connectionName,
            (x) => { return new Lazy<IConnection>(() => new ConnectionFactory().CreateConnection()); }).Value;
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
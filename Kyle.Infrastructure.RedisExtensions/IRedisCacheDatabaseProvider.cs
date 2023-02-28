using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface IRedisCacheDatabaseProvider
    {
        /// <summary>
        /// 连接池
        /// </summary>
        ConnectionMultiplexer ConnectionMultiplexer { get; }

        /// <summary>
        /// redis连接对象
        /// </summary>
        /// <returns></returns>
        IDatabase GetDatabase();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<IServer> GetServers();
    }

    public class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
    {
        private readonly KyleRedisCacheOptions _options;
        private readonly ILogger logger;

        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        private readonly object _lock = new object();

        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                if (_connectionMultiplexer.IsValueCreated)
                {
                    return _connectionMultiplexer.Value;
                }

                lock (_lock)
                {
                    if (_connectionMultiplexer.IsValueCreated)
                    {
                        return _connectionMultiplexer.Value;
                    }
                    return _connectionMultiplexer.Value;
                }
            }
        }

        public RedisCacheDatabaseProvider(ILogger<RedisCacheDatabaseProvider> logger, KyleRedisCacheOptions options)
        {
            this.logger = logger;
            _options = options;

            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer());

            _connectionMultiplexer.Value.ConnectionFailed += (sender, e) =>
            {
                throw new Exception("Redis to server connection Error!");
            };
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            var connectionString = _options.ConnectionString;
            logger.LogInformation($"RedisConnectionString: {connectionString}");

            //var config = ConfigurationOptions.Parse(connectionString);


            var connect = ConnectionMultiplexer.Connect(connectionString);
            return connect;
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
        }

        public IList<IServer> GetServers()
        {
            var list = new List<IServer>();
            var multiplexer = _connectionMultiplexer.Value;
            foreach (var endPoint in multiplexer.GetEndPoints())
            {
                var server = multiplexer.GetServer(endPoint);
                if (!server.IsConnected || !server.Features.Scan) continue;
                list.Add(server);
            }

            return list;
        }

    }
}

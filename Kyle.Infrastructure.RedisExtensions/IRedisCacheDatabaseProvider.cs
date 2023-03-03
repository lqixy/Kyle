using Kyle.Extensions.Exceptions;
using Microsoft.Extensions.Configuration;
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
        ConnectionMultiplexer ConnectionMultiplexer { get; }

        IDatabase GetDatabase();

    }

    public class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
    {
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly IConfiguration configuration;
        private readonly object _lock = new object();

        public RedisCacheDatabaseProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(GetConnectionMultiplexer());

            _connectionMultiplexer.Value.ConnectionFailed += (sender, e) =>
            {
                throw new KyleException("Redis connection error!");
            };
        }

        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                if (_connectionMultiplexer.IsValueCreated) return _connectionMultiplexer.Value;
                lock (_lock)
                {
                    if (_connectionMultiplexer.IsValueCreated) return _connectionMultiplexer.Value;

                    return _connectionMultiplexer.Value;
                }
            }
        }

        public IDatabase GetDatabase()
        {
            var databaseId = configuration["Redis:DatabaseId"];
            return _connectionMultiplexer.Value.GetDatabase();
        }

        private ConnectionMultiplexer GetConnectionMultiplexer()
        {
            var connectionStr = configuration["Redis:ConnectionString"];
            connectionStr ??= "localhost:6379,allowAdmin=true,defaultDatabase=10";
            var options = ConfigurationOptions.Parse(connectionStr);

            return ConnectionMultiplexer.Connect(options);
        }
    }
}

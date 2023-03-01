using Kyle.Extensions.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface IRedisCache
    {
        IDatabase Database { get; }

        //TimeSpan DefaultSlidingExpireTime { get; }

        //DateTimeOffset? DefaultAbsoluteExpireTime { get; }

        bool Exists(string key);
        Task<bool> ExistsAsync(string key);
        TimeSpan? GetExpiration(string key);
        Task<TimeSpan?> GetExpirationAsync(string key);
        object GetOrDefault(string key);
        void Remove(string key);
        Task RemoveAsync(string key);
        void Set(string key, object value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);
        void SetExpiration(string key, TimeSpan expiration);
        Task SetExpirationAsync(string key, TimeSpan expiration);
        bool TryGetValue(string key, out object value);
    }

    public class RedisCache : IRedisCache
    {
        private readonly IDatabase _database;
        //private readonly ILogger _logger;
        public IDatabase Database { get { return _database; } }

        private TimeSpan DefaultSlidingExpireTime { get; }

        private DateTimeOffset? DefaultAbsoluteExpireTime { get; }
        //private TimeSpan DefaultSlidingExpireTime => TimeSpan.FromHours(1);

        //private DateTimeOffset? DefaultAbsoluteExpireTime
        //{
        //    get
        //    {
        //        var now = DateTime.Now.AddDays(1);
        //        return DateTime.SpecifyKind(now, DateTimeKind.Utc);
        //    }
        //}

        private readonly IRedisCacheDatabaseProvider _databaseProvider;

        public RedisCache(IRedisCacheDatabaseProvider databaseProvider//, ILogger<RedisCache> logger
            )
        {
            _databaseProvider = databaseProvider;
            _database = _databaseProvider.GetDatabase();
            //_logger = logger;

            DefaultSlidingExpireTime = TimeSpan.FromHours(1);

            var now = DateTime.Now.AddDays(1);
            DefaultAbsoluteExpireTime = DateTime.SpecifyKind(now, DateTimeKind.Utc);
        }


        public bool TryGetValue(string key, out object value)
        {
            var redisValue = _database.StringGet(key);
            value = redisValue.HasValue ? JsonConvert.DeserializeObject(redisValue) : null;
            return redisValue.HasValue;
        }

        public object GetOrDefault(string key)
        {
            var obj = _database.StringGet(key);
            if (obj.HasValue)
            {
                return JsonConvert.DeserializeObject(obj);
            }
            return null;
        }

        public void Set(string key, object value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            if (value == null) throw new KyleException("Can not insert null values to the cache!");
            var redisValue = JsonConvert.SerializeObject(value);
            if (absoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(key, redisValue)) { }
                    //_logger.LogError($"Unable to set key:{key} value:{redisValue} in redis");
                else if (!_database.KeyExpire(key, absoluteExpireTime.Value.UtcDateTime)) { }
                    //_logger.LogError($"Unable to set key:{key} to expire at {absoluteExpireTime.Value.UtcDateTime} in Redis");
            }
            else if (slidingExpireTime.HasValue)
            {
                if (!_database.StringSet(key, redisValue, slidingExpireTime.Value)) { }
                    //_logger.LogError($"Unable to set key:{key} value:{redisValue} to expire after {slidingExpireTime.Value} in Redis");
            }
            else if (DefaultAbsoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(key, redisValue))
                {
                    //_logger.LogError("Unable to set key:{0} value:{1} in Redis", key, redisValue);
                }
                else if (!_database.KeyExpire(key, DefaultAbsoluteExpireTime.Value.UtcDateTime))
                {
                    //_logger.LogError("Unable to set key:{0} to expire at {1:O} in Redis", key, DefaultAbsoluteExpireTime.Value.UtcDateTime);
                }
            }
            else
            {
                if (!_database.StringSet(key, redisValue, DefaultSlidingExpireTime))
                {
                    //_logger.LogError("Unable to set key:{0} value:{1} to expire after {2:c} in Redis", key, redisValue, DefaultSlidingExpireTime);
                }
            }

        }

        public void Remove(string key)
        {
            _database.KeyDeleteAsync(key);
        }
        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public bool Exists(string key)
        {
            return _database.KeyExists(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return _database.KeyExistsAsync(key);
        }

        public void SetExpiration(string key, TimeSpan expiration)
        {
            if (expiration.Ticks < 0) this.Remove(key);
            _database.KeyExpire(key, expiration);
        }

        public Task SetExpirationAsync(string key, TimeSpan expiration)
        {
            if (expiration.Ticks < 0) this.RemoveAsync(key);
            return _database.KeyExpireAsync(key, expiration);
        }

        public Task<TimeSpan?> GetExpirationAsync(string key)
        {
            return _database.KeyTimeToLiveAsync(key);
        }

        public TimeSpan? GetExpiration(string key)
        {
            return _database.KeyTimeToLive(key);
        }

    }
}

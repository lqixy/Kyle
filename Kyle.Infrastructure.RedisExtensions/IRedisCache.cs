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
    public interface IRedisCache : IAbpCache<string, object>
    {
        IDatabase Database { get; }

        bool Exists(string key);
        Task<bool> ExistsAsync(string key);
        TimeSpan? GetExpiration(string key);
        Task<TimeSpan?> GetExpirationAsync(string key);
        object GetOrDefault(string key);
        void Remove(string key);
        Task RemoveAsync(string key);
        //void Set(string key, object value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);
        void SetExpiration(string key, TimeSpan expiration);
        Task SetExpirationAsync(string key, TimeSpan expiration);
        bool TryGetValue(string key, out object value);


    }

    public class RedisCache : AbpCacheBase<string, object>, IRedisCache
    {
        private readonly IDatabase _database;
        private readonly ILogger _logger;
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

        public RedisCache(IRedisCacheDatabaseProvider databaseProvider
            //, ILogger<RedisCache> logger
            , ILoggerFactory loggerFactory
            ) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RedisCache>();
            _databaseProvider = databaseProvider;
            _database = _databaseProvider.GetDatabase();
            //_logger = logger;

            DefaultSlidingExpireTime = TimeSpan.FromHours(1);

            var now = DateTime.Now.AddDays(1);
            DefaultAbsoluteExpireTime = DateTime.SpecifyKind(now, DateTimeKind.Utc);
        }


        public override bool TryGetValue(string key, out object value)
        {
            var redisValue = _database.StringGet(GetLocalizeKey(key));
            value = redisValue.HasValue ? JsonConvert.DeserializeObject(redisValue) : null;
            return redisValue.HasValue;
        }

        public object GetOrDefault(string key)
        {
            var obj = _database.StringGet(GetLocalizeKey(key));
            if (obj.HasValue)
            {
                return JsonConvert.DeserializeObject(obj);
            }
            return null;
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            if (value == null) throw new KyleException("Can not insert null values to the cache!");
            var redisKey = GetLocalizeKey(key);
            var redisValue = JsonConvert.SerializeObject(value);

            _logger.LogInformation($"Set key:{redisKey} value:{redisValue} in redis");

            if (absoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue))
                {
                    _logger.LogError($"Unable to set key:{redisKey} value:{redisValue} in redis");
                }
                else if (!_database.KeyExpire(redisKey, absoluteExpireTime.Value.UtcDateTime))
                {
                    _logger.LogError($"Unable to set key:{redisKey} to expire at {absoluteExpireTime.Value.UtcDateTime} in Redis");
                }
            }
            else if (slidingExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue, slidingExpireTime.Value))
                    _logger.LogError($"Unable to set key:{redisKey} value:{redisValue} to expire after {slidingExpireTime.Value} in Redis");
            }
            else if (DefaultAbsoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue))
                {
                    _logger.LogError("Unable to set key:{0} value:{1} in Redis", redisKey, redisValue);
                }
                else if (!_database.KeyExpire(redisKey, DefaultAbsoluteExpireTime.Value.UtcDateTime))
                {
                    _logger.LogError("Unable to set key:{0} to expire at {1:O} in Redis", redisKey, DefaultAbsoluteExpireTime.Value.UtcDateTime);
                }
            }
            else
            {
                if (!_database.StringSet(redisKey, redisValue, DefaultSlidingExpireTime))
                {
                    _logger.LogError("Unable to set key:{0} value:{1} to expire after {2:c} in Redis", redisKey, redisValue, DefaultSlidingExpireTime);
                }
            }

        }

        public void Remove(string key)
        {
            _database.KeyDeleteAsync(GetLocalizeKey(key));
        }
        public Task RemoveAsync(string key)
        {
            Remove(GetLocalizeKey(key));
            return Task.CompletedTask;
        }

        public bool Exists(string key)
        {
            return _database.KeyExists(GetLocalizeKey(key));
        }

        public Task<bool> ExistsAsync(string key)
        {
            return _database.KeyExistsAsync(GetLocalizeKey(key));
        }

        public void SetExpiration(string key, TimeSpan expiration)
        {
            if (expiration.Ticks < 0) this.Remove(GetLocalizeKey(key));
            _database.KeyExpire(key, expiration);
        }

        public Task SetExpirationAsync(string key, TimeSpan expiration)
        {
            if (expiration.Ticks < 0) this.RemoveAsync(GetLocalizeKey(key));
            return _database.KeyExpireAsync(key, expiration);
        }

        public Task<TimeSpan?> GetExpirationAsync(string key)
        {
            return _database.KeyTimeToLiveAsync(GetLocalizeKey(key));
        }

        public TimeSpan? GetExpiration(string key)
        {
            return _database.KeyTimeToLive(GetLocalizeKey(key));
        }

        private string GetLocalizeKey(string key)
        {
            return "Mall:" + key;
        }

    }
}

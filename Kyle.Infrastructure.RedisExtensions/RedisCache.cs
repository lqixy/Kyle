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
    public class RedisCache : CacheBase, IRedisCache
    {
        public string Name { get; }
        /// <summary>
        /// redis服务对象
        /// </summary>
        private readonly IDatabase _database;

        private readonly ILogger _logger;

        public IDatabase Database
        {
            get
            {
                return _database;
            }
        }

        //public DateTimeOffset? DefaultAbsoluteExpireTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public TimeSpan? DefaultSlidingExpireTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private readonly IRedisCacheDatabaseProvider redisCacheDatabaseProvider;

        public RedisCache(IRedisCacheDatabaseProvider redisCacheDatabaseProvider, ILogger logger) : this("Kyle.Redis", redisCacheDatabaseProvider, logger)
        {
        }

        public RedisCache(string name, IRedisCacheDatabaseProvider redisCacheDatabaseProvider, ILogger logger) : base(logger)
        {
            this.Name = name;
            this.redisCacheDatabaseProvider = redisCacheDatabaseProvider;
            _database = redisCacheDatabaseProvider.GetDatabase();
            _logger = logger;
        }

        public void DeletePatternKey(string pattern)
        {
            var keys = GetHashPatterKeys(pattern);
            Database.KeyDelete(keys.Select(x => x).ToArray());
        }

        public async Task DeletePatternKeyAsync(string pattern)
        {
            GeoRadiusResult[] result = Database.GeoRadius("", "", 123);

            var keys = GetHashPatterKeys(pattern);
            await Database.KeyDeleteAsync(keys.ToArray());
        }

        public bool Exists(string key)
        {
            return Database.KeyExists(GetLocalizedRedisKey(key));
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Database.KeyDeleteAsync(GetLocalizedRedisKey(key));
        }

        public void SetExpiration(string key, TimeSpan expiresIn)
        {
            if (expiresIn.Ticks < 0) this.Remove(GetLocalizedRedisKey(key));

            Database.KeyExpire(GetLocalizedRedisKey(key), expiresIn);
        }
        public override void Remove(string key)
        {
            _database.KeyDeleteAsync(GetLocalizedRedisKey(key));
        }
        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public Task SetExpirationAsync(string key, TimeSpan expiresIn)
        {
            if (expiresIn.Ticks < 0) RemoveAsync(GetLocalizedRedisKey(key));
            return Database.KeyExpireAsync(GetLocalizedRedisKey(key), expiresIn);
        }

        public Task<TimeSpan?> GetExpirationAsync(string key)
        {
            return Database.KeyTimeToLiveAsync(GetLocalizedRedisKey(key));
        }

        public TimeSpan? GetExpiration(string key)
        {
            return Database.KeyTimeToLive(GetLocalizedRedisKey(key));
        }

        public List<string> GetPatterKeys(string pattern)
        {
            var keys = GetHashPatterKeys(pattern + "*");
            return keys.Select(x => x.ToString()).ToList();
        }

        public long? GetSequence(string key)
        {
            var obj = _database.StringGet(GetLocalizedRedisKey(key));
            if (obj.HasValue) return (long)obj;
            return null;
        }

        private HashSet<RedisKey> GetHashPatterKeys(string pattern)
        {
            var keys = new HashSet<RedisKey>();

            var servers = redisCacheDatabaseProvider.GetServers();
            foreach (var server in servers)
            {
                var dbKeys = server.Keys(Database.Database, pattern);
                foreach (var dbKey in dbKeys)
                {
                    if (!keys.Contains(dbKey)) keys.Add(dbKey);
                }
            }
            return keys;
        }

        protected virtual RedisKey GetLocalizedRedisKey(string key)
        {
            return "n:" + Name + ",c:" + key;
        }

        public object GetOrDefault(string key)
        {
            var obj = _database.StringGet(GetLocalizedRedisKey(key));
            if (obj.HasValue) return Newtonsoft.Json.JsonConvert.DeserializeObject(obj);
            return null;
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            if (value == null) throw new ArgumentNullException("Can not insert null values to the cache!");

            var redisKey = GetLocalizedRedisKey(key);
            var redisValue = JsonConvert.SerializeObject(value);

            if (absoluteExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue))
                    _logger.LogError($"Unable to set key:{redisKey} value:{redisValue} in Redis");
                else if (!_database.KeyExpire(redisKey, absoluteExpireTime.Value.UtcDateTime))
                    _logger.LogError("Unable to set key:{0} to expire at {1:O} in Redis", redisKey, absoluteExpireTime.Value.UtcDateTime);
            }
            else if (slidingExpireTime.HasValue)
            {
                if (!_database.StringSet(redisKey, redisValue, slidingExpireTime.Value))
                    _logger.LogError("Unable to set key:{0} value:{1} to expire after {2:c} in Redis", redisKey, redisValue, slidingExpireTime.Value);
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

        public override bool TryGetValue(string key, out object value)
        {
            var redisValue = _database.StringGet(GetLocalizedRedisKey(key));
            value = redisValue.HasValue ? JsonConvert.DeserializeObject(redisValue) : null;
            return redisValue.HasValue;
        }

        //public override void Clear()
        //{
        //    _database.keyde
        //}
    }
}

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface IRedisCacheManager
    {
        IRedisCache GetCache();
        //IDatabase GetCache();
    }

    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly IRedisCache _redisCache;

        public RedisCacheManager(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        //public IDatabase Database => _redisCache.Database;
        public IRedisCache GetCache()
        {
            return this._redisCache;
        }
    }

}

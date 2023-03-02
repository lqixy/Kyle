using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public static class RedisDatabaseExtensions
    {
        public static ITypedCache<Tkey, TValue> AsTyped<Tkey, TValue>(this IRedisCache cache)
        {
            return new TypedCacheWrapper<Tkey, TValue>(cache);
        }
    }

    public static class CacheManagerExtensions
    {
        public static ITypedCache<TKey, TValue> GetCache<TKey, TValue>(this IRedisCacheManager cacheManager)
        {
            return cacheManager.GetCache().AsTyped<TKey, TValue>();
        }
    }
}

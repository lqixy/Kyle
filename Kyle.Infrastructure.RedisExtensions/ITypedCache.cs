using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface ITypedCache<TKey, TValue> : IAbpCache<TKey, TValue>
    {
        IRedisCache InternalCache { get; }
         
    }

    public class TypedCacheWrapper<TKey, TValue> : ITypedCache<TKey, TValue>
    {
        public IRedisCache InternalCache { get; private set; }

        public TypedCacheWrapper(IRedisCache internalCache)
        {
            InternalCache = internalCache;
        }

        public TValue Get(TKey key, Func<TKey, TValue> factory)
        {
            return (TValue)InternalCache.Get(key.ToString(), (k) => factory(key));
        }

        public async Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory)
        {
            return (TValue)await InternalCache.GetAsync(key.ToString(), async (k) => await factory(key));
        }

        public void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            InternalCache.Set(key.ToString(), value, slidingExpireTime, absoluteExpireTime);
        }

        public Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            return InternalCache.SetAsync(key.ToString(), value, slidingExpireTime, absoluteExpireTime);
        }

        //public IDatabase Database => throw new NotImplementedException();
    }
}

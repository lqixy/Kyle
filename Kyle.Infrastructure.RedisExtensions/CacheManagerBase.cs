using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public abstract class CacheManagerBase : ICacheManager
    {

        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase()
        {
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        public ICache GetCache(string key)
        {
            return Caches.GetOrAdd(key, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);
                return cache;
            });
        }

        protected abstract ICache CreateCacheImplementation(string name);
    }
}

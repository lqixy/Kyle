using Kyle.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface IAbpCache<TKey, TValue> : IAbpCache
    {
        TValue Get(TKey key, Func<TKey, TValue> factory);

        TValue[] Get(TKey[] keys, Func<TKey, TValue> factory);

        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        Task<TValue[]> GetAsync(TKey[] keys, Func<TKey, Task<TValue>> factory);

        bool TryGetValue(TKey key, out TValue value);

        TValue GetOrDefault(TKey key);

        TValue[] GetOrDefault(TKey[] keys);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Cached item or null if not found</returns>
        Task<TValue> GetOrDefaultAsync(TKey key);

        /// <summary>
        /// Gets items from the cache. For every key that is not found, a null value is returned.
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <returns>Cached items</returns>
        Task<TValue[]> GetOrDefaultAsync(TKey[] keys);

        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

        void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Result to indicate cache hit and cache item</returns>
        Task<ConditionalValue<TValue>> TryGetValueAsync(TKey key);

        /// <summary>
        /// Gets items from the cache.
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <returns>Results to indicate cache hit and cache item for each key</returns>
        ConditionalValue<TValue>[] TryGetValues(TKey[] keys);

        /// <summary>
        /// Gets items from the cache.
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <returns>Results to indicate cache hit and cache item for each key</returns>
        Task<ConditionalValue<TValue>[]> TryGetValuesAsync(TKey[] keys);


        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// Use one of the expire times at most (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>).
        /// If none of them is specified, then
        /// <see cref="ICacheOptions.DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="ICacheOptions.DefaultSlidingExpireTime"/>
        /// will be used.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="slidingExpireTime">Sliding expire time</param>
        /// <param name="absoluteExpireTime">Absolute expire time</param>
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

        /// <summary>
        /// Saves/Overrides items in the cache by the pairs.
        /// Use one of the expire times at most (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>).
        /// If none of them is specified, then
        /// <see cref="ICacheOptions.DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="ICacheOptions.DefaultSlidingExpireTime"/>
        /// will be used.
        /// </summary>
        /// <param name="pairs">Pairs</param>
        /// <param name="slidingExpireTime">Sliding expire time</param>
        /// <param name="absoluteExpireTime">Absolute expire time</param>
        Task SetAsync(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

        /// <summary>
        /// Removes a cache item by it's key (does nothing if given key does not exists in the cache).
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(TKey key);

        /// <summary>
        /// Removes cache items by their keys.
        /// </summary>
        /// <param name="keys">Keys</param>
        void Remove(TKey[] keys);

        /// <summary>
        /// Removes a cache item by it's key (does nothing if given key does not exists in the cache).
        /// </summary>
        /// <param name="key">Key</param>
        Task RemoveAsync(TKey key);

        /// <summary>
        /// Removes cache items by their keys.
        /// </summary>
        /// <param name="keys">Keys</param>
        Task RemoveAsync(TKey[] keys);
    }

    public interface IAbpCache
    {

        ///// <summary>
        ///// Clears all items in this cache.
        ///// </summary>
        //void Clear();

        ///// <summary>
        ///// Clears all items in this cache.
        ///// </summary>
        //Task ClearAsync();
    }

    public abstract class AbpCacheBase<TKey, TValue> : AbpCacheBase, IAbpCache<TKey, TValue>
    {
        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public DateTimeOffset? DefaultAbsoluteExpireTime { get; set; }

        protected readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        //private readonly ILogger _logger;

        protected AbpCacheBase(ILogger logger) : base(logger)
        {
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }

        public TValue Get(TKey key, Func<TKey, TValue> factory)
        {
            if (TryGetValue(key, out TValue value)) { return value; }

            using (SemaphoreSlim.Lock())
            {
                if (TryGetValue(key, out value)) return value;

                var generatedValue = factory(key);
                if (!IsDefaultValue(generatedValue))
                {

                    Set(key, generatedValue);

                }
                return generatedValue;
            }
        }

        public TValue[] Get(TKey[] keys, Func<TKey, TValue> factory)
        {
            ConditionalValue<TValue>[] results = null;

            results = TryGetValues(keys);

            if (results == null)
            {
                results = new ConditionalValue<TValue>[keys.Length];
            }

            if (results.Any(result => !result.HasValue))
            {
                using (SemaphoreSlim.Lock())
                {

                    results = TryGetValues(keys);


                    var generated = new List<KeyValuePair<TKey, TValue>>();
                    for (var i = 0; i < results.Length; i++)
                    {
                        var result = results[i];
                        if (!result.HasValue)
                        {
                            var key = keys[i];
                            var generatedValue = factory(key);
                            results[i] = new ConditionalValue<TValue>(true, generatedValue);

                            if (!IsDefaultValue(generatedValue))
                            {
                                generated.Add(new KeyValuePair<TKey, TValue>(key, generatedValue));
                            }
                        }
                    }

                    if (generated.Any())
                    {
                        Set(generated.ToArray());
                    }
                }
            }

            return results.Select(result => result.Value).ToArray();
        }

        public async Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory)
        {
            ConditionalValue<TValue> result = default;

            result = await TryGetValueAsync(key);

            if (result.HasValue)
            {
                return result.Value;
            }

            using (await SemaphoreSlim.LockAsync())
            {
                result = await TryGetValueAsync(key);

                if (result.HasValue)
                {
                    return result.Value;
                }

                var generatedValue = await factory(key);
                if (IsDefaultValue(generatedValue))
                {
                    return generatedValue;
                }

                await SetAsync(key, generatedValue);

                return generatedValue;
            }
        }

        public async Task<TValue[]> GetAsync(TKey[] keys, Func<TKey, Task<TValue>> factory)
        {
            ConditionalValue<TValue>[] results = null;

            results = await TryGetValuesAsync(keys);


            if (results == null)
            {
                results = new ConditionalValue<TValue>[keys.Length];
            }

            if (results.Any(result => !result.HasValue))
            {
                using (await SemaphoreSlim.LockAsync())
                {
                    results = await TryGetValuesAsync(keys);


                    var generated = new List<KeyValuePair<TKey, TValue>>();
                    for (var i = 0; i < results.Length; i++)
                    {
                        var result = results[i];
                        if (!result.HasValue)
                        {
                            var key = keys[i];
                            var generatedValue = await factory(key);
                            if (!IsDefaultValue(generatedValue))
                            {
                                generated.Add(new KeyValuePair<TKey, TValue>(key, generatedValue));
                            }
                            results[i] = new ConditionalValue<TValue>(true, generatedValue);
                        }
                    }

                    if (generated.Any())
                    {
                        await SetAsync(generated.ToArray());


                    }
                }
            }

            return results.Select(result => result.Value).ToArray();
        }

        public TValue GetOrDefault(TKey key)
        {
            TryGetValue(key, out TValue value);
            return value;
        }

        public TValue[] GetOrDefault(TKey[] keys)
        {
            var results = TryGetValues(keys);
            return results.Select(result => result.Value).ToArray();
        }

        public async Task<TValue> GetOrDefaultAsync(TKey key)
        {
            var result = await TryGetValueAsync(key);
            return result.Value;
        }

        public async Task<TValue[]> GetOrDefaultAsync(TKey[] keys)
        {
            var results = await TryGetValuesAsync(keys);
            return results.Select(result => result.Value).ToArray();
        }

        public abstract void Remove(TKey key);

        public void Remove(TKey[] keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public Task RemoveAsync(TKey key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(TKey[] keys)
        {
            return Task.WhenAll(keys.Select(RemoveAsync));
        }

        public abstract void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

        public virtual void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            foreach (var pair in pairs)
            {
                Set(pair.Key, pair.Value, slidingExpireTime, absoluteExpireTime);
            }
        }

        public virtual Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            Set(key, value, slidingExpireTime, absoluteExpireTime);
            return Task.CompletedTask;
        }

        public Task SetAsync(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            return Task.WhenAll(pairs.Select(p => SetAsync(p.Key, p.Value, slidingExpireTime, absoluteExpireTime)));
        }

        public abstract bool TryGetValue(TKey key, out TValue value);


        protected virtual bool IsDefaultValue(TValue value)
        {
            return EqualityComparer<TValue>.Default.Equals(value, default);
        }

        public Task<ConditionalValue<TValue>> TryGetValueAsync(TKey key)
        {
            var found = TryGetValue(key, out TValue value);
            return Task.FromResult(new ConditionalValue<TValue>(found, value));
        }

        public ConditionalValue<TValue>[] TryGetValues(TKey[] keys)
        {
            var pairs = new List<ConditionalValue<TValue>>();
            foreach (var key in keys)
            {
                var found = TryGetValue(key, out TValue value);
                pairs.Add(new ConditionalValue<TValue>(found, value));
            }
            return pairs.ToArray();
        }

        public Task<ConditionalValue<TValue>[]> TryGetValuesAsync(TKey[] keys)
        {
            return Task.FromResult(TryGetValues(keys));
        }
    }


    public abstract class AbpCacheBase : IDisposable, IAbpCache
    {
        public ILogger Logger { get; set; }


        protected AbpCacheBase(ILogger logger)
        {
            Logger = logger;
        }

        //public abstract void Clear();

        //public virtual Task ClearAsync()
        //{
        //    Clear();
        //    return Task.CompletedTask;
        //}

        public virtual void Dispose()
        {

        }
    }


}

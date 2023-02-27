using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface ICacheManager : IDisposable
    {
        IReadOnlyList<ICache> GetAllCaches();

        ICache GetCache(string key);
    }

    public interface ICache : IDisposable, ICacheOptions, IAbpCache<string, object>
    {

    }

    public interface ICacheOptions
    {
        /// <summary>
        /// Default sliding expire time of cache items.
        /// Default value: 60 minutes (1 hour).
        /// Can be changed by configuration.
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// Default absolute expire time of cache items.
        /// Default value: null (not used).
        /// </summary>
        DateTimeOffset? DefaultAbsoluteExpireTime { get; set; }
    }

    public interface IAbpCache<TKey, TValue> : IAbpCache
    {
        /// <summary>
        /// Gets an item from the cache.
        /// This method hides cache provider failures (and logs them),
        /// uses the factory method to get the object if cache provider fails.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="factory">Factory method to create cache item if not exists</param>
        /// <returns>Cached item</returns>
        TValue Get(TKey key, Func<TKey, TValue> factory);

        /// <summary>
        /// Gets items from the cache.
        /// This method hides cache provider failures (and logs them),
        /// uses the factory method to get the object if cache provider fails.
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <param name="factory">Factory method to create cache item if not exists</param>
        /// <returns>Cached item</returns>
        TValue[] Get(TKey[] keys, Func<TKey, TValue> factory);

        /// <summary>
        /// Gets an item from the cache.
        /// This method hides cache provider failures (and logs them),
        /// uses the factory method to get the object if cache provider fails.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="factory">Factory method to create cache item if not exists</param>
        /// <returns>Cached item</returns>
        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        /// <summary>
        /// Gets items from the cache.
        /// This method hides cache provider failures (and logs them),
        /// uses the factory method to get the object if cache provider fails.
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <param name="factory">Factory method to create cache item if not exists</param>
        /// <returns>Cached items</returns>
        Task<TValue[]> GetAsync(TKey[] keys, Func<TKey, Task<TValue>> factory);

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Cache item</param>
        /// <returns>Result to indicate cache hit</returns>
        bool TryGetValue(TKey key, out TValue value);

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
        /// Gets an item from the cache or null if not found.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Cached item or null if not found</returns>
        TValue GetOrDefault(TKey key);

        /// <summary>
        /// Gets items from the cache. For every key that is not found, a null value is returned.
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <returns>Cached items</returns>
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
        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

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
        void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

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
        /// <summary>
        /// Unique name of the cache.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Clears all items in this cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears all items in this cache.
        /// </summary>
        Task ClearAsync();
    }
    /// Reference from https://github.com/microsoft/service-fabric/blob/c326b801c6c709f36684700edfe7bb88ceec9d7f/src/prod/src/managed/Microsoft.ServiceFabric.Data.Interfaces/ConditionalResult.cs
    /// <summary>
    /// Result class that may or may not return a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value returned by this <cref name="ConditionalValue{TValue}"/>.</typeparam>
    public struct ConditionalValue<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <cref name="ConditionalValue{TValue}"/> class with the given value.
        /// </summary>
        /// <param name="hasValue">Indicates whether the value is valid.</param>
        /// <param name="value">The value.</param>
        public ConditionalValue(bool hasValue, TValue value)
        {
            HasValue = hasValue;
            Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <cref name="ConditionalValue{TValue}"/> object has a valid value of its underlying type.
        /// </summary>
        /// <returns><languageKeyword>true</languageKeyword>: Value is valid, <languageKeyword>false</languageKeyword> otherwise.</returns>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value of the current <cref name="ConditionalValue{TValue}"/> object if it has been assigned a valid underlying value.
        /// </summary>
        /// <returns>The value of the object. If HasValue is <languageKeyword>false</languageKeyword>, returns the default value for type of the TValue parameter.</returns>
        public TValue Value { get; }
    }

}

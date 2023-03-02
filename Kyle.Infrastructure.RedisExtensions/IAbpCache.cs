using Kyle.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface IAbpCache<TKey, TValue>
    {

        TValue Get(TKey key, Func<TKey, TValue> factory);

        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);
    }
    //public interface IAbpCache { }

    public abstract class AbpCacheBase<TKey, TValue> : IAbpCache<TKey, TValue>
    {
        private readonly ILogger logger;

        protected AbpCacheBase(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger<AbpCacheBase<TKey, TValue>>();
        }

        protected readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public abstract bool TryGetValue(TKey key, out TValue value);

        public virtual Task<ConditionalValue<TValue>> TryGetValueAsync(TKey key)
        {
            var fond = TryGetValue(key, out TValue value);
            return Task.FromResult(new ConditionalValue<TValue>(fond, value));
        }

        protected virtual bool IsDefaultValue(TValue value)
        {
            return EqualityComparer<TValue>.Default.Equals(value, default);
        }

        public abstract void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);

        public virtual Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null)
        {
            Set(key, value, slidingExpireTime, absoluteExpireTime);
            return Task.CompletedTask;
        }


        public virtual TValue Get(TKey key, Func<TKey, TValue> factory)
        {
            if (TryGetValue(key, out TValue value)) return value;

            using (SemaphoreSlim.Lock())
            {
                if (TryGetValue(key, out value)) return value;

                var generatedValue = factory(key);
                if (!IsDefaultValue(generatedValue))
                {
                    try
                    {
                        Set(key, generatedValue);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message, ex);
                    }
                }
                return generatedValue;
            }

        }

        public virtual async Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory)
        {
            ConditionalValue<TValue> result = default;
            try
            {
                result = await TryGetValueAsync(key);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }

            if (result.HasValue) return result.Value;

            using (await SemaphoreSlim.LockAsync())
            {
                try
                {
                    result = await TryGetValueAsync(key);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }

                if (result.HasValue) return result.Value;

                var generatedValue = await factory(key);
                if (IsDefaultValue(generatedValue)) return generatedValue;

                try
                {
                    await SetAsync(key, generatedValue);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }

                return generatedValue;
            }
        }

    }



    //public abstract class AbpCache<TKey, TValue> //: IAbpCache<TKey, TValue>
    //{
    //    public virtual TValue Get(TKey key, Func<TKey, TValue> factory)
    //    {

    //    }
    //}

}

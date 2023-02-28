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
        string Name { get; }
        IDatabase Database { get; }

        //DateTimeOffset? DefaultAbsoluteExpireTime { get; set; } 
        //TimeSpan? DefaultSlidingExpireTime { get; set; }
        object GetOrDefault(string key);

        void Remove(string key);

        Task RemoveAsync(string key);

        void Set(string key, object value, TimeSpan? slidingExpireTime = null, DateTimeOffset? absoluteExpireTime = null);



        /// <summary>
        /// 删除模糊匹配的Key
        /// </summary>
        /// <param name="pattern"></param>
        void DeletePatternKey(string pattern);

        /// <summary>
        /// 删除模糊匹配的Key
        /// </summary>
        /// <param name="pattern"></param>
        Task DeletePatternKeyAsync(string pattern);

        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 设置KEY的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        void SetExpiration(string key, TimeSpan expiresIn);

        /// <summary>
        /// 设置KEY的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        Task SetExpirationAsync(string key, TimeSpan expiresIn);

        /// <summary>
        /// 获取key的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TimeSpan?> GetExpirationAsync(string key);

        /// <summary>
        /// 获取key的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TimeSpan? GetExpiration(string key);
        /// <summary>
        /// 获取匹配的key值
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        List<string> GetPatterKeys(string pattern);

        /// <summary>
        /// 获取序列的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long? GetSequence(string key);

    }
}

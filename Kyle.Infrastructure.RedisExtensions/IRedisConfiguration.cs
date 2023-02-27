using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface IRedisConfiguration
    {
        /// <summary>
        /// 组合redis key值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GroupKeys(params string[] key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string GroupKeys(List<string> keys);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public class RedisConfiguration : IRedisConfiguration
    {
        public static RedisConfiguration Default { get { return Instance; } }
        private static readonly RedisConfiguration Instance = new RedisConfiguration();

        public string GroupKeys(params string[] key)
        {
            return string.Join(":", key.Where(x => x != null));
        }

        public string GroupKeys(List<string> keys)
        {
            return string.Join(":", keys.Where(x => x != null));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public class RedisCacheItem
    {
        public Type Type { get; set; }

        public string Item { get; set; }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface ICache : IDisposable, IAbpCache<string, object>
    {
    }

    public abstract class CacheBase : AbpCacheBase<string, object>, ICache
    {
        protected CacheBase(ILogger logger) : base(logger)
        {
        }
    }
}

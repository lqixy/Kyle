using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.RedisExtensions
{
    public interface ICacheManager //: IDisposable
    {
        //IReadOnlyList<ICache> GetAllCaches();

        ICache GetCache(string key);
    }

    public class RedisCacheManager : ICacheManager
    {
        //private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;

        public RedisCacheManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //public RedisCacheManager(IServiceCollection services)
        //{
        //    _services = services;
        //}

        //public void Dispose()
        //{
            
        //}

        //public IReadOnlyList<ICache> GetAllCaches()
        //{
        //    GetCache(string.Empty);
        //}

        public ICache GetCache(string name)
        {
            return _serviceProvider.GetRequiredService<RedisCache>();
        }
    }

    //public interface ICache //: IDisposable, ICacheOptions, IAbpCache<string, object>
    //{

    //}
     
}

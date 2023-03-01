using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Kyle.Infrastructure.RedisExtensions
{
    public static class RedisServiceExtensions
    {
        public static void AddRedisService(this IServiceCollection services)
        {

            services.AddSingleton<IRedisCacheDatabaseProvider, RedisCacheDatabaseProvider>();
            services.AddSingleton<IRedisCache, RedisCache>();

        }

    }
}

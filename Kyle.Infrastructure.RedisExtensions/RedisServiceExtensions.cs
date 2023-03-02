using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Kyle.Infrastructure.RedisExtensions
{
    public static class RedisServiceExtensions
    {
        public static void AddRedisService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(x => x.IsClass && x.IsAbstract);

            foreach (var type in types)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IAbpCache<,>))
                {
                    var interfaceType = typeof(IAbpCache<,>).MakeGenericType(type.GetGenericArguments());
                    services.AddSingleton(interfaceType, type);
                }
            }

            services.AddSingleton<IRedisCacheSerializer, RedisCacheSerializer>();
            services.AddSingleton<IRedisCacheDatabaseProvider, RedisCacheDatabaseProvider>();
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();

        }

    }
}

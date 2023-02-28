using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace Kyle.Infrastructure.RedisExtensions
{
    public static class RedisServiceExtensions
    {
        public static void AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<KyleRedisCacheOptions>();
            //services.AddSingleton<IRedisConfiguration, RedisConfiguration>();
            //services.AddSingleton<IRedisCacheDatabaseProvider, RedisCacheDatabaseProvider>();

            //services.AddSingleton<IRedisCache, RedisCache>();
            //services.AddSingleton<ICacheManager, RedisCacheManager>();

            //services.AddStackExchangeRedisExtensions();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
                options.InstanceName = "kyle:";
            });

            //var redisOptions = ConfigurationOptions.Parse(configuration["Redis:Configuration"]);
            //services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(redisOptions));

        }

        //public static T GetOrDefault<T>(this IDistributedCache cache, string key) where T : class
        //{
        //    var result = cache.GetString(key);
        //    if (result == null)
        //    {
        //        cache
        //    }
        //}

    }
}

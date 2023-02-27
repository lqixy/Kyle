using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Kyle.Infrastructure.RedisExtensions
{
    public static class RedisServiceExtensions
    {
        public static void AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
                options.InstanceName = "kyle:";
            });
        }
    }
}

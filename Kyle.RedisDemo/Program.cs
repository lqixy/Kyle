// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Kyle.Infrastructure.RedisExtensions;
using Kyle.RedisDemo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .Build()
    ;
using var logFactory = LoggerFactory.Create(logBuilder =>
{
    logBuilder.SetMinimumLevel(LogLevel.Trace);
});

var services = new ServiceCollection()
    .AddLogging(config => config.AddConsole())
    .AddSingleton<IConfiguration>(configuration)
    ;


services.AddRedisService();


var provider = services.BuildServiceProvider();


var cache = provider.GetService<IRedisCacheManager>();

var result = cache.GetCache<string, User>().Get("user", (key) =>
{
    return new User(2, "test");
});

//var result = cache.GetOrDefault("test");
//if (result == null)
//    cache.Set("test", new { Id = 1, Name = "kyle", CreationTime = DateTime.Now });

Console.WriteLine(result);


class User
{
    public User()
    {
    }

    public User(int id, string name)
    {
        Id = id;
        Name = name;
        CreationTime = DateTime.Now;
    }


    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime CreationTime { get; set; }
}

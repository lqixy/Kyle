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
    .AddSingleton<IConfiguration>(configuration)
    ;

services.AddRedisService();

// services.AddSingleton<ITest, Test>();

// Console.WriteLine(configuration["Redis:ConnectionString"]);

var provider = services.BuildServiceProvider();

// var t = provider.GetRequiredService<ITest>();
// t.Foo();

var cache = provider.GetService<IRedisCache>();

cache.Set("test", new { Id = 1, Name = "kyle", CreationTime = DateTime.Now });

var result = cache.GetOrDefault("test");
Console.WriteLine(result);


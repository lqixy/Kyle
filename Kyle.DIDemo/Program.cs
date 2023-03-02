// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Kyle.DIDemo;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

var services = new ServiceCollection();


var assembly = Assembly.GetEntryAssembly();

var types = assembly.GetTypes()
    .Where(x => x.IsClass && !x.IsAbstract);

foreach (var type in types)
{
    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ITest<,>))
    {
        var interfaceType = typeof(ITest<,>).MakeGenericType(type.GetGenericArguments());
        services.AddSingleton(interfaceType, type);
    }
}


//services.AddSingleton<ICache, Cache>();
services.AddSingleton<ICache, RedisCache>();


var provider = services.BuildServiceProvider();

var c = provider.GetRequiredService<ICache>();

c.Foo();
c.Test();



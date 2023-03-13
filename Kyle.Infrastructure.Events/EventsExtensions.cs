using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Infrastructure.Events;

public static class EventsExtensions
{
    public static void AddEvents(this ContainerBuilder builder)
    {
        builder.RegisterType<EventBus>().As<IEventBus>()
            .SingleInstance();
    }

    public static void AddEvents(this IServiceCollection services)
    {
        services.AddTransient<IMessagePublisher, MessagePublisher>();
    }
}
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Infrastructure.Mediators;

public static class MediatorServiceExtensions
{
    public static void AddMediator(this ContainerBuilder builder)
    {
        var mediatrConfig = MediatRConfigurationBuilder
            .Create(Extensions.AssemblyExtensions.GetAssemblies())
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        builder.RegisterMediatR(mediatrConfig);
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(Extensions.AssemblyExtensions.GetAssemblies());
        });
    }
}
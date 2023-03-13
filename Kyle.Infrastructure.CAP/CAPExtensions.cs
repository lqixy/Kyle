using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Infrastructure.CAP;

public static class CAPExtensions
{
    public static void AddCAPService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(options =>
        {
            options.UseSqlServer(configuration["ConnectionStrings:Default"]);
            // options.UseInMemoryStorage();

            options.UseRabbitMQ(mqOptions =>
            {
                mqOptions.HostName = configuration["RabbitMQ:HostName"];
                mqOptions.UserName = configuration["RabbitMQ:UserName"];
                mqOptions.Password = configuration["RabbitMQ:Password"];
                mqOptions.VirtualHost = configuration["RabbitMQ:VirtualHost"];
                // mqOptions.ExchangeName = "Q-Test-Exchange";
            });
            var retryCount = 5;
            int.TryParse(configuration["CAP:FailedRetryCount"], out retryCount);
            options.FailedRetryCount = retryCount;
            options.UseDashboard(o => o.PathMatch = "/cap");
        });
    }

    // public static void AddCAPAutofac(this ContainerBuilder builder)
    // {
    //     builder.RegisterType<CapPublisher>().As<ICapPublisher>()
    // }
}
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.ConsulFramework
{
    public static class ConsulExtensions
    {

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var client = new ConsulClient(c =>
            {
                c.Address = new Uri(configuration["Consul:Address"]);
            });

            var serviceId = Guid.NewGuid().ToString();

            var ip = configuration["Consul:IP"];
            var port = configuration["Consul:Port"];

            var registration = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = configuration["Consul:ServiceName"],
                Address = ip,
                Port = int.Parse(port),
                Check = new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{ip}:{port}/{configuration["Consul:Health"]}"
                }
            };

            client.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                client.Agent.ServiceDeregister(serviceId).Wait();
            });
            return app;
        }

    }
}

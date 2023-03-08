using IdentityModel.Client;
using Kyle.Identity.Application.Constructs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Identity.Application.Test
{
    public class TestBase
    {
        protected IServiceProvider provider;

        public TestBase()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                ;

            var services = new ServiceCollection();
            //services.AddSingleton<IIdentityClientAppService, IdentityClientAppService>();
            services.AddHttpClient();
            services.AddIdentityClient(options =>
            {
                options.ClientId = config["AuthServer:ClientId"];
                options.ClientSecret = config["AuthServer:ClientSecret"];
            });
            services.AddSingleton<IDiscoveryCache>(provider =>
            {
                var factory = provider.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(config["AuthServer:Authority"], () => factory.CreateClient(), policy: new DiscoveryPolicy
                {
                    RequireHttps = false
                });
            });


            provider = services.BuildServiceProvider();
        }
    }
}

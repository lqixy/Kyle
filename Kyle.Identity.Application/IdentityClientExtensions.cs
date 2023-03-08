using IdentityModel.Client;
using Kyle.Identity.Application.Constructs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Identity.Application
{
    public static class IdentityClientExtensions
    {
        public static void AddIdentityClient(this ServiceCollection services, Action<IdentityClientOptions> configureOptions)
        {
            services.AddSingleton<IIdentityClientAppService, IdentityClientAppService>();
            services.Configure(configureOptions);
            //services.AddSingleton<IDiscoveryCache>(provider =>
            //{
            //    return new DiscoveryCache()
            //});
        }
    }
}

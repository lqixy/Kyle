using IdentityModel.Client;
using Kyle.Identity.Application;
using Kyle.Identity.Application.Constructs;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.IdentityClient.Test
{
    [TestClass]
    public class IdentityClientTest
    {
        public IdentityClientAppService Init()
        {
            var options = new Mock<IOptions<IdentityClientOptions>>();
            options.Setup(x => x.Value).Returns(new IdentityClientOptions { });

            var httpFactory = new Mock<IHttpClientFactory>();
            httpFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient { BaseAddress = new Uri("http://localhost:5100") });

            var discovery = new DiscoveryCache("http://localhost:5100", () => httpFactory.Object.CreateClient());
            return new IdentityClientAppService(httpFactory.Object, discovery, options.Object);
        }


    }
}

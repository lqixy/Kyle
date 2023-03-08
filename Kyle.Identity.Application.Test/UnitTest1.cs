using Kyle.Identity.Application.Constructs;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Identity.Application.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        private readonly IIdentityClientAppService _appService;

        public UnitTest1()
        {
            _appService = provider.GetRequiredService<IIdentityClientAppService>();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var result = await _appService.Authorization("kyle", "123456");
        }
    }
}
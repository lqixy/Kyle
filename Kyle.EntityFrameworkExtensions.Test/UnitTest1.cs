using Kyle.EntityFrameworkExtensions.Test.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.EntityFrameworkExtensions.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        private readonly UserInfoRepository _userInfoRepository;

        public UnitTest1()
        {
            _userInfoRepository = Provider.GetRequiredService<UserInfoRepository>();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var list = await _userInfoRepository.Get();
            Assert.IsNotNull(list);
        }
    }
}
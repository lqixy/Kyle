using Kyle.Members.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Members.EntityFramework.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        private readonly IUserQueryRepository repository;

        public UnitTest1()
        {
            repository = provider.GetRequiredService<IUserQueryRepository>();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var result = await repository.Get();
        }
    }
}
using Autofac;
using Kyle.Extensions.Exceptions;
using Kyle.Infrastructure.TestBase;
using Kyle.Members.Application.Constructs;
using Kyle.Members.Domain;

namespace Kyle.Members.Application.Test;

[TestClass]
public class UserQueryTest: TestBase
{
    private readonly IUserAppService appService;

    public UserQueryTest()
    {
        appService = Container.Resolve<IUserAppService>();
    }

    [TestMethod]
    public async Task When_GetUser_Should_OK()
    {
        var user = await appService.Get();
        Assert.IsNotNull(user);
    }

    [TestMethod]
    public async Task When_GetUser_Should_Exception()
    {
        Assert.ThrowsExceptionAsync<UserFriendlyException>(async()=>await appService.Get());
    }
}
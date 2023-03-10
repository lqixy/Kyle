using Autofac;
using Kyle.Extensions.Exceptions;
using Kyle.Infrastructure.TestBase;
using Kyle.Members.Application.Constructs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Application.Test
{
    [TestClass]
    public class UserRegisterTest : TestBase
    {

        private readonly IUserRegisterAppService _userRegisterAppService;

        public UserRegisterTest()
        {
            _userRegisterAppService = Container.Resolve<IUserRegisterAppService>();
            //_userRegisterAppService = Provider.GetRequiredService<IUserRegisterAppService>();
        }


        [TestMethod]
        public async Task When_User_Register_Should_OK()
        {
            //var input = new Mock<RegisterInputDto>();
            //input.Setup().
            var input = new RegisterInputDto()
            {
                UserName = "tom",
                Password = "123456"
            };
            await _userRegisterAppService.Register(input);

            //await Assert.ThrowsExceptionAsync<UserFriendlyException>(async () => await _userRegisterAppService.Register(input));
        }
    }
}

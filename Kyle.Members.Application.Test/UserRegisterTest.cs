using Autofac;
using Kyle.Infrastructure.TestBase;
using Kyle.Members.Application.Constructs;
using Moq;
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
        }


        [TestMethod]
        public async Task When_User_Register_Should_OK()
        {
            //var input = new Mock<RegisterInputDto>();
            //input.Setup().
            await _userRegisterAppService.Register(new RegisterInputDto()
            {
                UserName = "kyle",
                Password = "123456"
            });
        }
    }
}

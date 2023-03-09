using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kyle.Mall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : MallControllerBase
    {
        private readonly IUserAppService _userAppService;

        public AccountController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task Register(RegisterInputDto input)
        {

        }

    }
}

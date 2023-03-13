using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kyle.Mall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : MallControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IUserRegisterAppService _registerAppService;
        public AccountController(IUserAppService userAppService, IUserRegisterAppService registerAppService)
        {
            _userAppService = userAppService;
            _registerAppService = registerAppService;
        }
        
        [HttpPost("register")]
        public async Task Register(RegisterInputDto input)
        {
            await _registerAppService.Register(input);
        }

    }
}

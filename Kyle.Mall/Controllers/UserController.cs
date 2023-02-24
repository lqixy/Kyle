using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kyle.Mall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService appService;

        public UserController(IUserAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var dto = await appService.Get();
            return new JsonResult(dto);
        }
    }
}

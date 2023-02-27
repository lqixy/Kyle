using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Kyle.Mall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService appService;
        private readonly ILogger<UserController> logger;
        private readonly IDistributedCache _cache;
        public UserController(IUserAppService appService, ILogger<UserController> logger, IDistributedCache cache)
        {
            this.appService = appService;
            this.logger = logger;
            _cache = cache;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        { 
            var dto = await appService.Get();
            logger.LogError("Test Error");
            logger.LogDebug("Test Debug");
            logger.LogInformation("Test Information");
            return new JsonResult(dto);
        }
    }
}

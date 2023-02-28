using Kyle.Infrastructure.RedisExtensions;
using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Kyle.Mall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService appService;
        private readonly ILogger<UserController> logger;
        //private readonly Lazy<ICacheManager> manager;
        private readonly IDistributedCache cache;
        public UserController(IUserAppService appService, ILogger<UserController> logger, IDistributedCache cache)
        {
            this.appService = appService;
            this.logger = logger;
            this.cache = cache;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var dto = await appService.Get();
            var cacheResult = cache.GetString(dto.UserId.ToString());
            if (cacheResult == null) cache.SetString(dto.UserId.ToString(), JsonConvert.SerializeObject(dto));
            //var result = await manager.Value.GetCache("test").GetAsync("test", async (key) =>
            // {
            //     return await appService.Get();
            // });
            //logger.LogError("Test Error");
            //logger.LogDebug("Test Debug");
            //logger.LogInformation("Test Information");
            return new JsonResult(dto);
        }
    }
}

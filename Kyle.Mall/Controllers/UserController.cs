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
    public class UserController : MallControllerBase
    {
        private readonly IUserAppService appService;
        private readonly Lazy<IRedisCacheManager> manager;
        public UserController(IUserAppService appService, Lazy<IRedisCacheManager> manager)
        {
            this.appService = appService;
            this.manager = manager;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var result = await manager.Value.GetCache<string, UserInfoDto>().GetAsync("user", async (k) =>
            {
                return await appService.Get();
            });

            //var dto = await appService.Get();
            //var cacheResult = cache.GetString(dto.UserId.ToString());
            //if (cacheResult == null) cache.SetString(dto.UserId.ToString(), JsonConvert.SerializeObject(dto));
            //var result = await manager.Value.GetCache("test").GetAsync("test", async (key) =>
            // {
            //     return await appService.Get();
            // });
            //logger.LogError("Test Error");
            //logger.LogDebug("Test Debug");
            //logger.LogInformation("Test Information");
            return new JsonResult(result);
        }
    }
}

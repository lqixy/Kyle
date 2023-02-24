using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kyle.Mall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            var dic = User.Claims.ToDictionary(k => k.Type, k => k.Value);

            return new JsonResult(JsonSerializer.Serialize(dic));
        }
    }
}

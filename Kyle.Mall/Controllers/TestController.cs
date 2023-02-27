using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kyle.Mall.Controllers
{
    [Route("api/test/[controller]")]
    [ApiController]
    //[Authorize]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public UserInfoDto Get()
        {
            var dic = User.Claims.ToDictionary(k => k.Type, k => k.Value);
            return new UserInfoDto(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            //return new JsonResult(JsonSerializer.Serialize(dic));
        }
    }
}

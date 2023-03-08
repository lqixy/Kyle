using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Identity.Application.Constructs
{
    public class AccessTokenDto
    {
        /// <summary>
        /// Accesstoken
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// refreshToken
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// token传递协议
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// token过期时间，秒
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 授权时间utc
        /// </summary>
        [JsonProperty(PropertyName = "issued")]
        public string IsSued { get; set; }

        /// <summary>
        /// 过期时间utc
        /// </summary>
        [JsonProperty(PropertyName = "expires")]
        public string Expires { get; set; }
    }
}

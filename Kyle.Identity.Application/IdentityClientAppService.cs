using Kyle.Identity.Application.Constructs;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Kyle.Identity.Application
{
    public class IdentityClientAppService : IIdentityClientAppService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDiscoveryCache _discoveryCache;
        private string clientId;
        private string clientSecret;

        public IdentityClientAppService(IHttpClientFactory httpClientFactory, IDiscoveryCache discoveryCache, IOptions<IdentityClientOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _discoveryCache = discoveryCache;
            clientId = options.Value.ClientId;
            clientSecret = options.Value.ClientSecret;
        }

        public async Task<AccessTokenDto> Authorization(string userName, string password)
        {
            var disco = await _discoveryCache.GetAsync();
            if (disco.IsError) { throw new Exception(disco.Error); }

            var client = _httpClientFactory.CreateClient();
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                UserName = userName,
                Password = password,
                //Scope =
            });
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            return new AccessTokenDto
            {
                AccessToken = tokenResponse.AccessToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                TokenType = tokenResponse.TokenType,
                RefreshToken = tokenResponse.RefreshToken
            };
        }


    }
}

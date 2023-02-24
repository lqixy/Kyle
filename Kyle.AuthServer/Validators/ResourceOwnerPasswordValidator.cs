using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;
using static IdentityModel.OidcConstants;

namespace Kyle.AuthServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userName = context.UserName;
            var password = context.Password;
            if (userName == null || password == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "账号或密码错误");
            }
            else
            {
                context.Result = new GrantValidationResult(Guid.NewGuid().ToString(), AuthenticationMethods.Password,
                    new Claim[]
                    {
                        new Claim("tenantId",Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.Role,"admin"),
                        new Claim(JwtClaimTypes.Name,userName),
                    });
            }

            return Task.CompletedTask;
        }
    }
}

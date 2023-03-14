using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;
using static IdentityModel.OidcConstants;

namespace Kyle.AuthServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentityUserPasswordValidatorFactory _factory;
        private readonly ILogger _logger;

        public ResourceOwnerPasswordValidator(IIdentityUserPasswordValidatorFactory factory,
            ILogger<ResourceOwnerPasswordValidator> logger)
        {
            _factory = factory;
            _logger = logger;

            _factory.Add<MallUserPasswordValidator>(MallUserPasswordValidator.OWNER_TYPE);
            _factory.Add<BackendUserPasswordValidator>(BackendUserPasswordValidator.OWNER_TYPE);
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            string ownerType = "";
            context.Request.Client.Properties?.TryGetValue("OwnerType", out ownerType);

            var userFinder = _factory.Get(ownerType);
            var result = await userFinder.CheckPasswordSignIn(context.UserName, context.Password);
            var user = result?.Result;

            if (user != null)
            {
                if (user.Succeeded)
                {
                    _logger.LogInformation($"Credentials validated for username:{context.UserName}");
                    context.Result = new GrantValidationResult(result.SubjectId, AuthenticationMethods.Password
                        , result.Claims);
                    return;
                }
                else if (user.IsLockedOut)
                {
                    _logger.LogInformation($"Authentication failed for username:{context.UserName},reason: locked out");
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription: "账号已被锁定");
                }
                else if (user.IsNotAllowed)
                {
                    _logger.LogInformation($"Authentication failed for username:{context.UserName},reason: not allowed");
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription: "账号不允许访问");
                }
                else
                {
                    _logger.LogInformation($"Authentication failed for username:{context.UserName},reason: invalid credentials");
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription: "账号或密码错误");
                }
            }
            else
            {
                _logger.LogInformation($"No user found matching username:{context.UserName}");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription: "账号或密码错误");
            }

        }
    }
}
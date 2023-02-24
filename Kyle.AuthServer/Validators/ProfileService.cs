using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Kyle.AuthServer
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger _logger;

        public ProfileService(ILogger<ProfileService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 期望为用户加载声明
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);
            context.IssuedClaims = context.Subject.Claims.ToList();
            context.LogIssuedClaims(_logger);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 预期用于指示当前是否允许用户获取令牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogInformation($"IsActive called from: {context.Caller}");
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Autofac;
using IdentityModel;
using Kyle.Extensions.Exceptions;
using Kyle.Members.Application.Constructs;
using Microsoft.AspNetCore.Identity;

namespace Kyle.AuthServer;

public interface IIdentityUserPasswordValidatorFactory
{
    void Add<T>(string targetType) where T : class, IIdentityUserSignIn;

    IIdentityUserSignIn Get(string targetType);
}

public class IdentityUserPasswordValidatorFactory : IIdentityUserPasswordValidatorFactory
{
    private readonly Dictionary<string, Type> dic = new Dictionary<string, Type>();
    // private readonly ContainerBuilder builder;
    // private readonly IContainer _container;

    private readonly IServiceProvider _provider;

    public IdentityUserPasswordValidatorFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void Add<T>(string targetType) where T : class, IIdentityUserSignIn
    {
        if (!dic.ContainsKey(targetType))

            dic.Add(targetType, typeof(T));
        // builder.RegisterType<T>().InstancePerLifetimeScope();
    }

    public IIdentityUserSignIn Get(string targetType)
    {
        if (dic.ContainsKey(targetType))
        {
            return _provider.GetService(dic[targetType]) as IIdentityUserSignIn;
        }

        throw new UserFriendlyException($"认证身份类型不支持 {targetType}");
    }
}

public interface IIdentityUserSignIn
{
    Task<IdentityUserSignInResult> CheckPasswordSignIn(string username, string password);
}

public class MallUserPasswordValidator : IIdentityUserSignIn
{
    public const string OWNER_TYPE = "Mall";
    private readonly IUserAppService _userAppService;

    public MallUserPasswordValidator(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    public async Task<IdentityUserSignInResult> CheckPasswordSignIn(string username, string password)
    {
        var user = await Find(username);
        if (user == null) return null;
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(user.Pwd))
        {
            return new IdentityUserSignInResult(SignInResult.Failed);
        }

        if (string.Compare(user.Pwd.Trim(), password.Trim(), true) != 0)
        {
            return new IdentityUserSignInResult(SignInResult.Failed);
        }

        if (user.DelStatus.HasValue)
        {
            return new IdentityUserSignInResult(SignInResult.NotAllowed);
        }

        return new IdentityUserSignInResult(SignInResult.Success)
        {
            SubjectId = $"{user.UserId}",
            Claims = new Claim[]
            {
                new Claim(JwtClaimTypes.Name, user.UserName ?? "not set"),
                new Claim(JwtClaimTypes.Role, "User"),
                new Claim("tenantId", user.TenantId.ToString())
            }
        };
    }

    private async Task<UserInfoDto> Find(string userId)
    {
        if (Guid.TryParse(userId, out Guid id))
        {
            return await _userAppService.Get(id);
        }

        throw new UserFriendlyException($"未找到用户: {userId}");
    }
}

public class BackendUserPasswordValidator : IIdentityUserSignIn
{
    public const string OWNER_TYPE = "Backend";
    private readonly IUserAppService _userAppService;

    public BackendUserPasswordValidator(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    public async Task<IdentityUserSignInResult> CheckPasswordSignIn(string username, string password)
    {
        var user = await Find(username);
        if (user == null) return null;
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(user.Pwd))
        {
            return new IdentityUserSignInResult(SignInResult.Failed);
        }

        if (string.Compare(user.Pwd.Trim(), password.Trim(), true) != 0)
        {
            return new IdentityUserSignInResult(SignInResult.Failed);
        }

        if (user.DelStatus.HasValue)
        {
            return new IdentityUserSignInResult(SignInResult.NotAllowed);
        }

        return new IdentityUserSignInResult(SignInResult.Success)
        {
            SubjectId = $"{user.UserId}",
            Claims = new Claim[]
            {
                new Claim(JwtClaimTypes.Name, user.UserName ?? "not set"),
                new Claim(JwtClaimTypes.Role, "Admin"),
                new Claim("tenantId", user.TenantId.ToString())
            }
        };
    }

    private async Task<UserInfoDto> Find(string userId)
    {
        if (Guid.TryParse(userId, out Guid id))
        {
            return await _userAppService.Get(id);
        }

        throw new UserFriendlyException($"未找到用户: {userId}");
    }
}

public class IdentityUserSignInResult
{
    public IdentityUserSignInResult(SignInResult result)
    {
        Result = result;
    }

    public SignInResult Result { get; set; }

    public string SubjectId { get; set; }

    public Claim[] Claims { get; set; }
}
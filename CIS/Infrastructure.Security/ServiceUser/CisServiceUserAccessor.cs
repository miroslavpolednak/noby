namespace CIS.Infrastructure.Security.ServiceUser;

internal sealed class CisServiceUserAccessor
    : CIS.Core.Security.IServiceUserAccessor
{
    private readonly Core.Security.IServiceUser? _user;
    private readonly IHttpContextAccessor _contextAccessor;

    public CisServiceUserAccessor(IHttpContextAccessor httpContext)
    {
        _contextAccessor = httpContext;
        _user = httpContext.HttpContext?.User?.Identities?.FirstOrDefault(t => t is CisServiceIdentity) as Core.Security.IServiceUser;
    }

    public Core.Security.IServiceUser? User
    {
        get => _user;
    }

    public bool IsInRole(in string roleName)
    {
        return IsAuthenticated ? _contextAccessor.HttpContext!.User.IsInRole(roleName) : false;
    }

    public bool IsAuthenticated
    {
        get => _user is not null;
    }
}

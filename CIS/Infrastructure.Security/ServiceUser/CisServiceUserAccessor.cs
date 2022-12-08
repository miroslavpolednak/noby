namespace CIS.Infrastructure.Security.ServiceUser;

internal sealed class CisServiceUserAccessor
    : CIS.Core.Security.IServiceUserAccessor
{
    private readonly Core.Security.IServiceUser? _user;

    public CisServiceUserAccessor(IHttpContextAccessor httpContext)
    {
        _user = httpContext.HttpContext?.User?.Identities?.FirstOrDefault(t => t is CisServiceIdentity) as Core.Security.IServiceUser;
    }

    public Core.Security.IServiceUser? User
    {
        get => _user;
    }

    public bool IsAuthenticated
    {
        get => _user is not null;
    }
}

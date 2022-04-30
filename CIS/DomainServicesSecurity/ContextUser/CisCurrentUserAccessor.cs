using CIS.Core.Security;

namespace CIS.DomainServicesSecurity.ContextUser;

internal class CisCurrentUserAccessor 
    : ICurrentUserAccessor
{
    private readonly ICurrentUser? _user;

    public CisCurrentUserAccessor(IHttpContextAccessor httpContext)
    {
        _user = httpContext.HttpContext?.User?.Identities?.FirstOrDefault(t => t is ICurrentUser) as ICurrentUser;
    }

    public ICurrentUser? User
    {
        get => _user;
    }

    public bool IsAuthenticated
    {
        get => _user is not null;
    }
}

using CIS.Core.Security;

namespace CIS.DomainServicesSecurity.ContextUser;

public sealed class CisCurrentContextUserAccessor 
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor? _httpContext;
    private ICurrentUser? _user;

    public CisCurrentContextUserAccessor(IHttpContextAccessor? httpContext)
    {
        _httpContext = httpContext;
    }

    public ICurrentUser? User
    {
        get 
        {
            if (_user is null)
                _user = _httpContext?.HttpContext?.User?.Identities?.FirstOrDefault(t => t is ICurrentUser) as ICurrentUser;
            return _user;
        }
    }

    public bool IsAuthenticated
    {
        get => User is not null;
    }
}

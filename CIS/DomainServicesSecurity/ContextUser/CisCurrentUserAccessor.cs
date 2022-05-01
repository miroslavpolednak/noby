using CIS.Core.Security;

namespace CIS.DomainServicesSecurity.ContextUser;

internal class CisCurrentUserAccessor 
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContext;
    private ICurrentUser? _user;

    public CisCurrentUserAccessor(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public ICurrentUser? User
    {
        get 
        {
            if (_user is null)
                _user = _httpContext.HttpContext?.User?.Identities?.FirstOrDefault(t => t is ICurrentUser) as ICurrentUser;
            return _user;
        }
    }

    public bool IsAuthenticated
    {
        get => User is not null;
    }
}

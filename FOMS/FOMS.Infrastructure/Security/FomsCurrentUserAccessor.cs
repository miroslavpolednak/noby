using CIS.Core.Security;
using Microsoft.AspNetCore.Http;

namespace FOMS.Infrastructure.Security;

public sealed class FomsCurrentUserAccessor 
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContext;
    private ICurrentUser? _user;

    public FomsCurrentUserAccessor(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public ICurrentUser? User
    {
        get
        {
            if (_user is null)
                _user = _httpContext.HttpContext?.User as ICurrentUser;
            return _user;
        }
    }

    public bool IsAuthenticated => User is not null;
}

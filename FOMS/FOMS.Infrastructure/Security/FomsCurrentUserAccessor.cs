using CIS.Core.Security;
using Microsoft.AspNetCore.Http;

namespace FOMS.Infrastructure.Security;

public sealed class FomsCurrentUserAccessor 
    : ICurrentUserAccessor
{
    private readonly ICurrentUser? _user;

    public FomsCurrentUserAccessor(IHttpContextAccessor? httpContext)
    {
        _user = httpContext?.HttpContext?.User as ICurrentUser;
    }

    public bool IsAuthenticated => _user is not null;

    public ICurrentUser? User => _user;
}

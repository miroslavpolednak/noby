using CIS.Core.Security;
using CIS.DomainServicesSecurity;

namespace CIS.InternalServices.ServiceDiscovery.Api;

internal sealed class ServiceDiscoveryContextUserAccessor
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContext;
    private ICurrentUser? _user;

    public ServiceDiscoveryContextUserAccessor(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public bool IsAuthenticated
    {
        get => User is not null;
    }

    public ICurrentUser? User
    {
        get
        {
            int? userId = CurrentUserAccessorHelpers.GetUserIdFromHeaders(_httpContext.HttpContext!.Request);
            if (userId.HasValue)
                _user = new ServiceDiscoveryContextUser(userId.Value);
            return _user;
        }
    }

    public ICurrentUserDetails? UserDetails => throw new NotImplementedException();

    public Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken)
        where TDetails : ICurrentUserDetails
    {
        throw new NotImplementedException();
    }
}

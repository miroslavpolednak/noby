using CIS.Core.Security;
using System.Security.Claims;

namespace CIS.Infrastructure.Security.ContextUser;

public sealed class CisCurrentContextUserAccessor 
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor? _httpContext;

    private ICurrentUser? _user;
    private ICurrentUserDetails? _userDetails;
    private bool _userDetailsFetched;

    public CisCurrentContextUserAccessor(IHttpContextAccessor? httpContext)
    {
        _httpContext = httpContext;
    }

    public IEnumerable<Claim> Claims
    {
#pragma warning disable CS8603 // Possible null reference return.
        get => _httpContext?.HttpContext?.User.Claims;
#pragma warning restore CS8603 // Possible null reference return.
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

    public ICurrentUserDetails? UserDetails
    {
        get => _userDetailsFetched ? _userDetails : throw new InvalidOperationException("Trying to access UserDetails without fetching details first. Call FetchDetails() to ensure data being loaded.");
    }

    public async Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
        {
            throw new InvalidOperationException("Missing authenticated user - can not fetch user details");
        }

        if (_userDetailsFetched)
        {
            return _userDetails!;
        }

        var userCache = _httpContext!.HttpContext!.RequestServices.GetRequiredService<CisCurrentUserAccessorCache>();
        _userDetails = await userCache.GetUser(_user!.Id, cancellationToken);
        _userDetailsFetched = true;

        return _userDetails;
    }
    
    public SharedTypes.Types.UserIdentity? GetMainIdentity()
    {
        return CurrentUserAccessorHelpers.GetUserIdentityFromHeaders(_httpContext?.HttpContext?.Request);
    }

    public async Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken = default) 
        where TDetails : ICurrentUserDetails
    {
        await EnsureDetails(cancellationToken);
        return (TDetails)_userDetails!;
    }
}

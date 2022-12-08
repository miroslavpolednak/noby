using CIS.Core.Results;
using CIS.Core.Security;

namespace CIS.Infrastructure.Security.ContextUser;

public sealed class CisCurrentContextUserAccessor 
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor? _httpContext;

    private ICurrentUser? _user;
    private Foms.Types.Interfaces.IFomsCurrentUserDetails? _userDetails;
    private bool _userDetailsFetched;

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

    public ICurrentUserDetails? UserDetails
    {
        get => _userDetailsFetched ? _userDetails : throw new InvalidOperationException("Trying to access UserDetails without fetching details first. Call FetchDetails() to ensure data being loaded.");
    }

    public async Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken = default(CancellationToken))
    {
        if (!IsAuthenticated)
            throw new InvalidOperationException("Missing authenticated user - can not fetch user details");
        if (_userDetailsFetched) return _userDetails!;

        _userDetailsFetched = true;

        var userService = _httpContext!.HttpContext!.RequestServices.GetRequiredService<DomainServices.UserService.Clients.IUserServiceClient>();
        var userInstance = await userService.GetUser(_user!.Id, cancellationToken);
        _userDetails = new FomsCurrentUserDetails
        {
            DisplayName = userInstance.FullName,
            CPM = userInstance.CPM,
            ICP = userInstance.ICP
        };

        return _userDetails;
    }

    public async Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken) 
        where TDetails : ICurrentUserDetails
    {
        if (typeof(TDetails) is not Foms.Types.Interfaces.IFomsCurrentUserDetails)
            throw new InvalidOperationException("User detail type must be of Foms.Types.Interfaces.IFomsCurrentUserDetails");

        await EnsureDetails(cancellationToken);
        return (TDetails)_userDetails!;
    }
}

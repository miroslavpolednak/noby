using CIS.Core.Results;
using CIS.Core.Security;

namespace CIS.DomainServicesSecurity.ContextUser;

public sealed class CisCurrentContextUserAccessor 
    : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor? _httpContext;
    private readonly DomainServices.UserService.Abstraction.IUserServiceAbstraction _userService;

    private ICurrentUser? _user;
    private ICurrentUserDetails? _userDetails;
    private bool _userDetailsFetched;

    public CisCurrentContextUserAccessor(IHttpContextAccessor? httpContext, DomainServices.UserService.Abstraction.IUserServiceAbstraction userService)
    {
        _httpContext = httpContext;
        _userService = userService;
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

        var userInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.UserService.Contracts.User>(await _userService.GetUser(_user!.Id, cancellationToken));
        _userDetails = new CisUserDetails
        {
            DisplayName = userInstance.FullName,
            CPM = userInstance.CPM,
            ICP = userInstance.ICP
        };

        return _userDetails;
    }
}

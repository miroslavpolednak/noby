using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Users.GetMpssSecurityCookie;

internal sealed class GetMpssSecurityCookieHandler
    : IRequestHandler<GetMpssSecurityCookieRequest, string>
{
    public async Task<string> Handle(GetMpssSecurityCookieRequest request, CancellationToken cancellationToken)
    {
        var u = await _userServiceClient.GetUser(_currentUser.User!.Id, cancellationToken);

        return _portal.CreateCookieValue(u.UserInfo.Cpm, u.UserInfo.Icp, u.UserInfo.DisplayName, u.UserId, 0, 0, 0, 0, 0);
    }

    private readonly MPSS.Security.Noby.IPortal _portal;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userServiceClient;

    public GetMpssSecurityCookieHandler(
        MPSS.Security.Noby.IPortal portal,
        DomainServices.UserService.Clients.IUserServiceClient userServiceClient, 
        ICurrentUserAccessor currentUser)
    {
        _portal = portal;
        _userServiceClient = userServiceClient;
        _currentUser = currentUser;
    }
}

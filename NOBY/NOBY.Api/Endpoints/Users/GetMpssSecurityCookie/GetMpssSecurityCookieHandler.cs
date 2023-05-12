using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Users.GetMpssSecurityCookie;

internal sealed class GetMpssSecurityCookieHandler
    : IRequestHandler<GetMpssSecurityCookieRequest, string>
{
    public async Task<string> Handle(GetMpssSecurityCookieRequest request, CancellationToken cancellationToken)
    {
        var u = await _userServiceClient.GetUser(_currentUser.User!.Id, cancellationToken);

        string? m17id = u.UserIdentifiers.FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.M17Id)?.Identity;
        string? brokerId = u.UserIdentifiers.FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.BrokerId)?.Identity;

        return _portal.CreateCookieValue(u.UserInfo.Cpm, u.UserInfo.Icp, u.UserInfo.DisplayName, u.UserId, 0, 0, 0, string.IsNullOrEmpty(m17id) ? 0 : Convert.ToInt32(m17id), string.IsNullOrEmpty(brokerId) ? 0 : Convert.ToInt32(brokerId));
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

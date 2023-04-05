using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Users.GetMpssSecurityCookie;

internal sealed class GetMpssSecurityCookieHandler
    : IRequestHandler<GetMpssSecurityCookieRequest, string>
{
    public async Task<string> Handle(GetMpssSecurityCookieRequest request, CancellationToken cancellationToken)
    {
        var u = await _userServiceClient.GetUser(_currentUser.User!.Id, cancellationToken);

        return MPSS.Security.Noby.Portal.CreateCookieValue(u.CPM, u.ICP, u.FullName, u.Id, 0, 0, 0, 0, 0, _httpContext.HttpContext!);
    }

    private readonly IHttpContextAccessor _httpContext;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userServiceClient;

    public GetMpssSecurityCookieHandler(
        IHttpContextAccessor httpContext,
        DomainServices.UserService.Clients.IUserServiceClient userServiceClient, 
        ICurrentUserAccessor currentUser)
    {
        _httpContext = httpContext;
        _userServiceClient = userServiceClient;
        _currentUser = currentUser;
    }
}

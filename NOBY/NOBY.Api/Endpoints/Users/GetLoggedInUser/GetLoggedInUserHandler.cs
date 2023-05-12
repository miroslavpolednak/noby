using Google.Protobuf.Collections;

namespace NOBY.Api.Endpoints.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserHandler
    : IRequestHandler<GetLoggedInUserRequest, GetLoggedInUserResponse>
{
    public async Task<GetLoggedInUserResponse> Handle(GetLoggedInUserRequest request, CancellationToken cancellationToken)
    {
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        return new GetLoggedInUserResponse
        {
            UserId = userInstance.UserId,
            UserInfo = new GetLoggedInUserResponseUserInfo
            {
                FirstName = userInstance.UserInfo.FirstName,
                LastName = userInstance.UserInfo.LastName,
                Cin = userInstance.UserInfo.Cin,
                Cpm = userInstance.UserInfo.Cpm,
                Icp = userInstance.UserInfo.Icp
            },
            UserIdentifiers = userInstance.UserIdentifiers.Select(t => (CIS.Foms.Types.UserIdentity)t!).ToList(),
            UserAttributes = new GetLoggedInUserResponseAttributes
            {
                EmailAddress = userInstance.UserAttributes?.Email,
                PhoneNumber = userInstance.UserAttributes?.PhoneNumber,
                IsUserVIP = userInstance.UserAttributes?.IsUserVIP ?? false
            },
            UserPermissions = getPermissions(userInstance.UserPermissions)
        };
    }

    private int[]? getPermissions(RepeatedField<int> permissions)
    {
        return null;
    }

    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public GetLoggedInUserHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.UserService.Clients.IUserServiceClient userService)
    {
        _userAccessor = userAccessor;
        _userService = userService;
    }
}

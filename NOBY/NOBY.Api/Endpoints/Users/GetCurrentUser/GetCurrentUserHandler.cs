namespace NOBY.Api.Endpoints.Users.GetCurrentUser;

internal sealed class GetCurrentUserHandler
    : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
{
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        return new GetCurrentUserResponse
        {
            Id = userInstance.UserId,
            Name = userInstance.UserInfo.DisplayName,
            CPM = userInstance.UserInfo.Cpm,
            ICP = userInstance.UserInfo.Icp
        };
    }

    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public GetCurrentUserHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.UserService.Clients.IUserServiceClient userService)
    {
        _userAccessor = userAccessor;
        _userService = userService;
    }
}
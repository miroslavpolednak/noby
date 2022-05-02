namespace FOMS.Api.Endpoints.Users.GetCurrentUser;

internal class GetCurrentUserHandler
    : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
{
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        _logger.UserGetCurrentUserInfo(_userAccessor.User!.Id);

        var userInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.UserService.Contracts.User>(await _userService.GetUser(_userAccessor.User!.Id, cancellationToken));

        return new GetCurrentUserResponse
        {
            Id = userInstance.Id,
            Name = userInstance.FullName,
            Username = userInstance.Login,
            CPM = userInstance.CPM,
            ICP = userInstance.ICP
        };
    }

    private readonly ILogger<GetCurrentUserHandler> _logger;
    private readonly DomainServices.UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public GetCurrentUserHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<GetCurrentUserHandler> logger, 
        DomainServices.UserService.Abstraction.IUserServiceAbstraction userService)
    {
        _userAccessor = userAccessor;
        _logger = logger;
        _userService = userService;
    }
}
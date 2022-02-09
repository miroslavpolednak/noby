using CIS.Core.Results;

namespace FOMS.Api.Endpoints.User.Handlers;

internal class GetCurrentUserHandler
    : IRequestHandler<Dto.GetCurrentUserRequest, Dto.GetCurrentUserResponse>
{
    public async Task<Dto.GetCurrentUserResponse> Handle(Dto.GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        //TODO get login from CAAS
        string login = "990614w";

        _logger.LogDebug("Get info about {login}", login);

        var userInstance = ServiceCallResult.Resolve<DomainServices.UserService.Contracts.User>(await _userService.GetUserByLogin(login, cancellationToken));

        return new Dto.GetCurrentUserResponse
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

    public GetCurrentUserHandler(ILogger<GetCurrentUserHandler> logger, DomainServices.UserService.Abstraction.IUserServiceAbstraction userService)
    {
        _logger = logger;
        _userService = userService;
    }
}
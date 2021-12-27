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

        var userInstance = resolveResult(await _userService.GetUserByLogin(login, cancellationToken));

        return new Dto.GetCurrentUserResponse
        {
            Id = userInstance.Id,
            Name = userInstance.FullName,
            Username = userInstance.Login
        };
    }

    private DomainServices.UserService.Contracts.User resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.UserService.Contracts.User> u => u.Model,
            _ => throw new NotImplementedException()
        };

    private readonly ILogger<GetCurrentUserHandler> _logger;
    private readonly DomainServices.UserService.Abstraction.IUserServiceAbstraction _userService;

    private GetCurrentUserHandler(ILogger<GetCurrentUserHandler> logger, DomainServices.UserService.Abstraction.IUserServiceAbstraction userService)
    {
        _logger = logger;
        _userService = userService;
    }
}
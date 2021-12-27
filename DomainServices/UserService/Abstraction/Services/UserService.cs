using CIS.Core.Results;
using Microsoft.Extensions.Logging;

namespace DomainServices.UserService.Abstraction.Services;

internal class UserService : IUserServiceAbstraction
{
    public async Task<IServiceCallResult> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetUserByLogin {login}", login);
        var result = await _userContext.AddUserContext(async () => await _service.GetUserByLoginAsync(
            new Contracts.GetUserByLoginRequest { 
                Login = login
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<Contracts.User>(result);
    }
    
    private readonly ILogger<UserService> _logger;
    private readonly Contracts.v1.UserService.UserServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

    public UserService(
        ILogger<UserService> logger,
        Contracts.v1.UserService.UserServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}

using CIS.Core.Results;

namespace DomainServices.UserService.Clients.Services;

internal class UserService : IUserServiceClient
{
    public async Task<IServiceCallResult> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var result = await _service.GetUserByLoginAsync(
                new Contracts.GetUserByLoginRequest
                {
                    Login = login
                }, cancellationToken: cancellationToken);
            return new SuccessfulServiceCallResult<Contracts.User>(result);
        }
        catch (CIS.Core.Exceptions.CisNotFoundException ex)
        {
            return new ErrorServiceCallResult(ex.ExceptionCode, ex.Message);
        }
    }

    public async Task<IServiceCallResult> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var result = await _service.GetUserAsync(
                new Contracts.GetUserRequest
                {
                    UserId = userId
                }, cancellationToken: cancellationToken);
            return new SuccessfulServiceCallResult<Contracts.User>(result);
        }
        catch (CIS.Core.Exceptions.CisNotFoundException ex)
        {
            return new ErrorServiceCallResult(ex.ExceptionCode, ex.Message);
        }
    }

    private readonly Contracts.v1.UserService.UserServiceClient _service;

    public UserService(Contracts.v1.UserService.UserServiceClient service)
    {
        _service = service;
    }
}

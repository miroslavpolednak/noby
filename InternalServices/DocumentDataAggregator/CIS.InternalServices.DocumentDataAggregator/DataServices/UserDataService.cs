using CIS.Core.Results;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices;

internal class UserDataService
{
    private readonly IUserServiceClient _userService;

    public UserDataService(IUserServiceClient userService)
    {
        _userService = userService;
    }

    public async Task<User> LoadData(int userId)
    {
        var result = await _userService.GetUser(userId);

        return ServiceCallResult.ResolveAndThrowIfError<User>(result);
    }
}
using CIS.Core.Results;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class UserServiceWrapper : IServiceWrapper
{
    private readonly IUserServiceClient _userService;

    public UserServiceWrapper(IUserServiceClient userService)
    {
        _userService = userService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.UserId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.UserId));

        var result = await _userService.GetUser(input.UserId.Value, cancellationToken);

        data.User = ServiceCallResult.ResolveAndThrowIfError<User>(result);
    }
}
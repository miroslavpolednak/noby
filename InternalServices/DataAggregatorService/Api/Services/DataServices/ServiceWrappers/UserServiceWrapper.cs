using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.UserService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class UserServiceWrapper : IServiceWrapper
{
    private readonly IUserServiceClient _userService;

    public UserServiceWrapper(IUserServiceClient userService)
    {
        _userService = userService;
    }

    public DataSource DataSource => DataSource.UserService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateUserId();

        var user = await _userService.GetUser(input.UserId!.Value, cancellationToken);

        data.User = new UserInfo(user);
    }
}
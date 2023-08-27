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

    public DataService DataService => DataService.UserService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateUserId();

        var user = await _userService.GetUser(input.UserId!.Value, cancellationToken);

        data.User.Source = user;
    }
}
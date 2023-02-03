﻿using DomainServices.UserService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

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

        data.User = await _userService.GetUser(input.UserId.Value, cancellationToken);
    }
}
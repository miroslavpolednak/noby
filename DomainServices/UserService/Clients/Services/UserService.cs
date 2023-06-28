namespace DomainServices.UserService.Clients.Services;

internal class UserService 
    : IUserServiceClient
{
    public async Task<Contracts.User> GetUser(string loginWithScheme, CancellationToken cancellationToken = default(CancellationToken))
    {
        var arr = loginWithScheme.Split('=');
        if (arr.Length != 2)
        {
            throw new ArgumentOutOfRangeException(nameof(loginWithScheme));
        }

        if (!Enum.TryParse<CIS.Foms.Enums.UserIdentitySchemes>(arr[0], true, out CIS.Foms.Enums.UserIdentitySchemes scheme))
        {
            throw new ArgumentOutOfRangeException(nameof(loginWithScheme));
        }

        return await GetUser(new CIS.Foms.Types.UserIdentity(arr[1], scheme), cancellationToken);
    }

    public async Task<Contracts.User> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await GetUser(new CIS.Foms.Types.UserIdentity(userId.ToString(), CIS.Foms.Enums.UserIdentitySchemes.V33Id), cancellationToken);
    }

    public async Task<Contracts.User> GetUser(CIS.Foms.Types.UserIdentity identity, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (identity.Scheme == CIS.Foms.Enums.UserIdentitySchemes.V33Id)
        {
            // pokud bude user nalezen v kesi
            if (_distributedCacheProvider.UseDistributedCache)
            {
                var cachedUser = await _distributedCacheProvider.DistributedCacheInstance!.GetAsync(Helpers.CreateUserCacheKey(identity.Identity), cancellationToken);
                if (cachedUser is not null)
                {
                    return Contracts.User.Parser.ParseFrom(cachedUser);
                }
            }
        }

        return await _service.GetUserAsync(
            new Contracts.GetUserRequest
            {
                Identity = identity,
            }, cancellationToken: cancellationToken);
    }

    public async Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        // pokud bude user nalezen v kesi
        if (_distributedCacheProvider.UseDistributedCache)
        {
            var cachedUser = await _distributedCacheProvider.DistributedCacheInstance!.GetAsync(Helpers.CreateUserPermissionsCacheKey(userId), cancellationToken);
            if (cachedUser is not null)
            {
                return Contracts.GetUserPermissionsResponse.Parser.ParseFrom(cachedUser).UserPermissions.ToArray();
            }
        }

        var response = await _service.GetUserPermissionsAsync(
            new Contracts.GetUserPermissionsRequest
            {
                UserId = userId,
            }, cancellationToken: cancellationToken);
        return response.UserPermissions.ToArray();
    }

    private readonly Contracts.v1.UserService.UserServiceClient _service;
    private readonly UserServiceClientCacheProvider _distributedCacheProvider;

    public UserService(Contracts.v1.UserService.UserServiceClient service, UserServiceClientCacheProvider distributedCacheProvider)
    {
        _distributedCacheProvider = distributedCacheProvider;
        _service = service;
    }
}

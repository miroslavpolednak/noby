using CIS.Core.Security;
using DomainServices.UserService.Contracts;

namespace DomainServices.UserService.Clients.Services;

internal class UserService 
    : IUserServiceClient
{
    public async Task<Contracts.User> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        return await GetUser(_currentUser.User!.Id, cancellationToken);
    }

    public async Task<Contracts.User> GetUser(string loginWithScheme, CancellationToken cancellationToken = default)
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

    public async Task<Contracts.User> GetUser(int userId, CancellationToken cancellationToken = default)
    {
        return await GetUser(new CIS.Foms.Types.UserIdentity(userId.ToString(), CIS.Foms.Enums.UserIdentitySchemes.V33Id), cancellationToken);
    }

    public async Task<Contracts.User> GetUser(CIS.Foms.Types.UserIdentity identity, CancellationToken cancellationToken = default)
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

    public async Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default)
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

    public async Task<UserRIPAttributes> GetUserRIPAttributes(string identity, string identityScheme, CancellationToken cancellationToken = default)
    {
        return await _service.GetUserRIPAttributesAsync(new GetUserRIPAttributesRequest { Identity = identity, IdentityScheme = identityScheme }, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.UserService.UserServiceClient _service;
    private readonly UserServiceClientCacheProvider _distributedCacheProvider;
    private readonly ICurrentUserAccessor _currentUser;

    public UserService(
        Contracts.v1.UserService.UserServiceClient service, 
        UserServiceClientCacheProvider distributedCacheProvider,
        ICurrentUserAccessor currentUser)
    {
        _currentUser = currentUser;
        _distributedCacheProvider = distributedCacheProvider;
        _service = service;
    }
}

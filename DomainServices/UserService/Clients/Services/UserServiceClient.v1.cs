using CIS.Core.Security;
using CIS.Infrastructure.Caching.Grpc;
using DomainServices.UserService.Clients.Dto;
using DomainServices.UserService.Contracts;
using System.Globalization;

namespace DomainServices.UserService.Clients.v1;

internal sealed class UserServiceClient(
	Contracts.v1.UserService.UserServiceClient _service,
    IGrpcClientResponseCache<UserServiceClient> _cache,
	ICurrentUserAccessor _currentUser)
    : IUserServiceClient
{
	public async Task<GetUserMortgageSpecialistResponse> GetUserMortgageSpecialist(int userId, CancellationToken cancellationToken = default)
    {
        return await _service.GetUserMortgageSpecialistAsync(new GetUserMortgageSpecialistRequest { UserId = userId }, cancellationToken: cancellationToken);
	}

	public async Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        return await GetUser(_currentUser.User!.Id, cancellationToken);
    }

    public async Task<GetUserBasicInfoResponse> GetUserBasicInfo(int userId, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            userId,
            async (c) => await GetUserBasicInfoWithoutCache(userId, c),
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<GetUserBasicInfoResponse> GetUserBasicInfoWithoutCache(int userId, CancellationToken cancellationToken = default)
    {
        return await _service.GetUserBasicInfoAsync(
            new()
            {
                UserId = userId
            }, cancellationToken: cancellationToken);
    }

    public async Task<UserDto> GetUser(int userId, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            userId,
            async (c) => await GetUser(new SharedTypes.Types.UserIdentity()
            {
                Scheme = SharedTypes.Enums.UserIdentitySchemes.V33Id,
                Identity = userId.ToString(CultureInfo.InvariantCulture),
            }, c),
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<UserDto> GetUser(string loginWithScheme, CancellationToken cancellationToken = default)
    {
        var arr = loginWithScheme.Split('=');
        if (arr.Length != 2)
        {
            throw new ArgumentOutOfRangeException(nameof(loginWithScheme));
        }

        if (!Enum.TryParse(arr[0], true, out SharedTypes.Enums.UserIdentitySchemes scheme))
        {
            throw new ArgumentOutOfRangeException(nameof(loginWithScheme));
        }

        return await GetUser(new SharedTypes.Types.UserIdentity(arr[1], scheme), cancellationToken);
    }

    public async Task<UserDto> GetUser(SharedTypes.Types.UserIdentity identity, CancellationToken cancellationToken = default)
    {
        return (await _service.GetUserAsync(
            new GetUserRequest
            {
                Identity = identity,
            }, cancellationToken: cancellationToken))
            .MapToDto();
    }

    public async Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default)
    {
        return await _cache.GetLocalOrDistributed(
            userId,
            async (c) => await GetUserPermissionsWithoutCache(userId, c),
            new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken);
    }

    public async Task<int[]> GetUserPermissionsWithoutCache(int userId, CancellationToken cancellationToken = default)
    {
        var response = await _service.GetUserPermissionsAsync(
            new GetUserPermissionsRequest
            {
                UserId = userId,
            }, cancellationToken: cancellationToken);
        return response.UserPermissions.ToArray();
    }

    public async Task<int[]> GetCurrentUserPermissions(CancellationToken cancellationToken = default)
    {
        return await GetUserPermissions(_currentUser.User!.Id, cancellationToken);
    }

    public async Task<UserRIPAttributes> GetUserRIPAttributes(string identity, string identityScheme, CancellationToken cancellationToken = default)
    {
        return await _service.GetUserRIPAttributesAsync(new GetUserRIPAttributesRequest { Identity = identity, IdentityScheme = identityScheme }, cancellationToken: cancellationToken);
    }
}

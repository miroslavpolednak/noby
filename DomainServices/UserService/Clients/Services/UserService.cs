namespace DomainServices.UserService.Clients.Services;

internal class UserService : IUserServiceClient
{
    public async Task<Contracts.User> GetUser(string id, string schema, CancellationToken cancellationToken = default(CancellationToken))
    {
        // pokud bude user nalezen v kesi
        if (_distributedCacheProvider.UseDistributedCache)
        {
            var cachedUser = await _distributedCacheProvider.DistributedCacheInstance!.GetAsync(Helpers.CreateUserCacheKey(userId), cancellationToken);
            if (cachedUser is not null)
            {
                return Contracts.User.Parser.ParseFrom(cachedUser);
            }
        }

        return await _service.GetUserAsync(
            new Contracts.GetUserRequest
            {
                Identity = new()
                {
                    Identity = id,
                    IdentityScheme = schema
                }
            }, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.UserService.UserServiceClient _service;
    private readonly UserServiceClientCacheProvider _distributedCacheProvider;

    public UserService(Contracts.v1.UserService.UserServiceClient service, UserServiceClientCacheProvider distributedCacheProvider)
    {
        _distributedCacheProvider = distributedCacheProvider;
        _service = service;
    }
}

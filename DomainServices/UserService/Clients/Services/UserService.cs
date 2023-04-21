namespace DomainServices.UserService.Clients.Services;

internal class UserService : IUserServiceClient
{
    public async Task<Contracts.User> GetUserByLogin(string login, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetUserByLoginAsync(
            new Contracts.GetUserByLoginRequest
            {
                Login = login
            }, cancellationToken: cancellationToken);
    }

    public async Task<Contracts.User> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken))
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
                UserId = userId
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

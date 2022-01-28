namespace DomainServices.UserService.Api.Handlers;

internal class GetUserByLoginHandler
    : IRequestHandler<Dto.GetUserByLoginMediatrRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Dto.GetUserByLoginMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get user {login}", request.Login);
        string cacheKey = Contracts.Helpers.GetUserCacheKey(request.Login);

        // pokud je uzivatel v kesi, vytahni ho
        if (_cache.Exists(cacheKey))
        {
            _logger.LogDebug("Getting user from cache");
            return await _cache.GetAsync<Contracts.User>(cacheKey, CIS.Infrastructure.Caching.SerializationTypes.Json) ?? throw new KeyNotFoundException(cacheKey);
        }

        // vytahnout info o uzivateli z DB
        var userInstance = await _repository.GetUser(request.Login);
        if (userInstance is null) // uzivatele se nepovedlo podle loginu najit
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.NotFound, $"User '{request.Login}' not found", 1);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            Id = userInstance!.v33id,
            CPM = userInstance.v33cpm ?? "",
            ICP = userInstance.v33icp ?? "",
            FullName = $"{userInstance.v33jmeno} {userInstance.v33prijmeni}".Trim(),
            Login = request.Login,
            Email = "",
            Phone = ""
        };

        // ulozit do kese
        _logger.LogDebug("Store user in cache");
        await _cache.SetAsync(cacheKey, model, CIS.Infrastructure.Caching.SerializationTypes.Json);

        return model;
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly ILogger<GetUserByLoginHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache _cache;

    public GetUserByLoginHandler(
        CIS.Infrastructure.Caching.IGlobalCache cache,
        Repositories.XxvRepository repository,
        ILogger<GetUserByLoginHandler> logger)
    {
        _cache = cache;
        _repository = repository;
        _logger = logger;
    }
}

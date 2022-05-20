namespace DomainServices.UserService.Api.Handlers;

internal class GetUserByLoginHandler
    : IRequestHandler<Dto.GetUserByLoginMediatrRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Dto.GetUserByLoginMediatrRequest request, CancellationToken cancellation)
    {
        string cacheKey = Contracts.Helpers.GetUserCacheKey(request.Login);
        var cachedUser = await _cache.GetObjectAsync<Dto.V33PmpUser>(cacheKey, SerializationTypes.Protobuf);

        // pokud je uzivatel v kesi, vytahni ho
        if (cachedUser is null)
        {
            // vytahnout info o uzivateli z DB
            cachedUser = await _repository.GetUser(request.Login);

            // ulozit do kese
            _logger.LogDebug("Store user in cache");
            await _cache.SetObjectAsync(cacheKey, cachedUser, _cacheOptions, SerializationTypes.Protobuf, cancellation);
        }

        if (cachedUser is null) // uzivatele se nepovedlo podle loginu najit
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.NotFound, $"User '{request.Login}' not found", 1);

        // vytvorit finalni model
        return new Contracts.User
        {
            Id = cachedUser!.v33id,
            CPM = cachedUser.v33cpm ?? "",
            ICP = cachedUser.v33icp ?? "",
            FullName = $"{cachedUser.v33jmeno} {cachedUser.v33prijmeni}".Trim(),
            Email = "",
            Phone = ""
        };
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly ILogger<GetUserByLoginHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly CIS.Infrastructure.Telemetry.IAuditLogger _audit;

    static DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(20) };

    public GetUserByLoginHandler(
        CIS.Infrastructure.Telemetry.IAuditLogger audit,
        IDistributedCache cache,
        Repositories.XxvRepository repository,
        ILogger<GetUserByLoginHandler> logger)
    {
        _audit = audit;
        _cache = cache;
        _repository = repository;
        _logger = logger;
    }
}

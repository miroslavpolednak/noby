using DomainServices.UserService.Contracts;

namespace DomainServices.UserService.Api.Endpoints.GetUserByLogin;

internal class GetUserByLoginHandler
    : IRequestHandler<GetUserByLoginRequest, User>
{
    public async Task<User> Handle(GetUserByLoginRequest request, CancellationToken cancellation)
    {
        string cacheKey = Helpers.GetUserCacheKey(request.Login);
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
        var model = new Contracts.User
        {
            Id = cachedUser!.v33id,
            CPM = cachedUser.v33cpm ?? "",
            ICP = cachedUser.v33icp ?? "",
            FullName = $"{cachedUser.v33jmeno} {cachedUser.v33prijmeni}".Trim(),
            Email = "",
            Phone = ""
        };

        model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.Mpad));

        // https://jira.kb.cz/browse/HFICH-2276
        if (model.CPM == "99999943")
            model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.BrokerId));

        return model;
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly ILogger<GetUserByLoginHandler> _logger;
    private readonly IDistributedCache _cache;

    static DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(20) };

    public GetUserByLoginHandler(
        IDistributedCache cache,
        Repositories.XxvRepository repository,
        ILogger<GetUserByLoginHandler> logger)
    {
        _cache = cache;
        _repository = repository;
        _logger = logger;
    }
}

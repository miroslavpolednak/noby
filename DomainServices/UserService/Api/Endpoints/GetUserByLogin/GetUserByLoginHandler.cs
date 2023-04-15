using DomainServices.UserService.Contracts;
using Google.Protobuf;

namespace DomainServices.UserService.Api.Endpoints.GetUserByLogin;

internal class GetUserByLoginHandler
    : IRequestHandler<GetUserByLoginRequest, User>
{
    public async Task<User> Handle(GetUserByLoginRequest request, CancellationToken cancellation)
    {
        var cachedUser = await _repository.GetUser(request.Login);

        if (cachedUser is null) // uzivatele se nepovedlo podle loginu najit
        {
            // ojebavka pro pripad loginu z CAASu
            cachedUser = await _repository.GetUser("990614w");
        }

        // vytvorit finalni model
        var model = new Contracts.User
        {
            Id = cachedUser!.v33id,
            CPM = cachedUser.v33cpm ?? "",
            ICP = cachedUser.v33icp ?? "",
            FullName = $"{cachedUser.v33jmeno} {cachedUser.v33prijmeni}".Trim(),
            Email = "",
            Phone = "",
            UserVip = false,
            CzechIdentificationNumber = "12345678"
        };

        model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.Mpad));

        // https://jira.kb.cz/browse/HFICH-2276
        if (model.CPM == "99999943")
            model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.BrokerId));

        if (_distributedCache is not null)
        {
            await _distributedCache.SetAsync(Helpers.CreateUserCacheKey(model.Id), model.ToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(1),
            },
            cancellation);
        }

        return model;
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly IDistributedCache? _distributedCache;

    public GetUserByLoginHandler(
        Repositories.XxvRepository repository,
        IDistributedCache? distributedCache)
    {
        _repository = repository;
        _distributedCache = distributedCache;
    }
}

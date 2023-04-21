using DomainServices.UserService.Contracts;
using Google.Protobuf;

namespace DomainServices.UserService.Api.Endpoints.GetUser;

internal class GetUserHandler
    : IRequestHandler<GetUserRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(GetUserRequest request, CancellationToken cancellation)
    {
        // zkusit cache
        string cacheKey = Helpers.CreateUserCacheKey(request.UserId);
        if (_distributedCache is not null)
        {
            var cachedBytes = await _distributedCache.GetAsync(cacheKey, cancellation);
            if (cachedBytes != null)
            {
                return Contracts.User.Parser.ParseFrom(cachedBytes);
            }
        }

        // vytahnout info o uzivateli z DB
        var userInstance = await _repository.GetUser(request.UserId);
        if (userInstance is null)
            throw new CIS.Core.Exceptions.CisNotFoundException(0, "User", request.UserId);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            Id = userInstance.v33id,
            CPM = "99806569", //Mock because of CheckForm/DV + CaseStateChanged; userInstance.v33cpm ?? "",
            ICP = "114306569", //Mock because of CheckForm/DV + CaseStateChanged; userInstance.v33icp ?? "",
            FullName = $"{userInstance.v33jmeno} {userInstance.v33prijmeni}".Trim(),
            Email = "",
            Phone = "",
            UserVip = false,
            CzechIdentificationNumber = "12345678"
        };

        model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity("A09FK3", CIS.Foms.Enums.UserIdentitySchemes.KbUid));

        model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.Mpad));

        // https://jira.kb.cz/browse/HFICH-2276
        if (model.CPM == "99999943")
            model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.BrokerId));

        if (_distributedCache is not null)
        {
            await _distributedCache.SetAsync(cacheKey, model.ToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(1),
            },
            cancellation);
        }

        return model;
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly IDistributedCache? _distributedCache;

    public GetUserHandler(
        Repositories.XxvRepository repository,
        IDistributedCache? distributedCache)
    {
        _repository = repository;
        _distributedCache = distributedCache;
    }
}

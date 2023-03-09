using DomainServices.UserService.Contracts;

namespace DomainServices.UserService.Api.Endpoints.GetUserByLogin;

internal class GetUserByLoginHandler
    : IRequestHandler<GetUserByLoginRequest, User>
{
    public async Task<User> Handle(GetUserByLoginRequest request, CancellationToken cancellation)
    {
        // na tvrdaka zadanej login, protoze nemame jak a kde zjistit mapovani caas identit na v33
        string login = "99999943";

        var cachedUser = await _repository.GetUser(login);

        if (cachedUser is null) // uzivatele se nepovedlo podle loginu najit
            throw new CIS.Core.Exceptions.CisNotFoundException(0, "User", login);

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

        return model;
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly ILogger<GetUserByLoginHandler> _logger;
    
    public GetUserByLoginHandler(
        Repositories.XxvRepository repository,
        ILogger<GetUserByLoginHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

using DomainServices.UserService.Api.Endpoints.GetUserByLogin;
using DomainServices.UserService.Contracts;

namespace DomainServices.UserService.Api.Endpoints.GetUser;

internal class GetUserHandler
    : IRequestHandler<GetUserRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(GetUserRequest request, CancellationToken cancellation)
    {
        // vytahnout info o uzivateli z DB
        var userInstance = await _repository.GetUser(request.UserId);
        if (userInstance is null)
            throw new CIS.Core.Exceptions.CisNotFoundException(0, "User", request.UserId);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            Id = userInstance.v33id,
            CPM = userInstance.v33cpm ?? "",
            ICP = userInstance.v33icp ?? "",
            FullName = $"{userInstance.v33jmeno} {userInstance.v33prijmeni}".Trim(),
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

    public GetUserHandler(
        Repositories.XxvRepository repository,
        ILogger<GetUserByLoginHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

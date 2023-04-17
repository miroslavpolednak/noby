﻿using DomainServices.UserService.Contracts;

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
            CPM = "99806569", //Mock because of CheckForm/DV + CaseStateChanged; userInstance.v33cpm ?? "",
            ICP = "114306569", //Mock because of CheckForm/DV + CaseStateChanged; userInstance.v33icp ?? "",
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

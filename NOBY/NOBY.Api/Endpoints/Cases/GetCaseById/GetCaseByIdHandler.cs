namespace NOBY.Api.Endpoints.Cases.GetCaseById;

internal sealed class GetCaseByIdHandler
    : IRequestHandler<GetCaseByIdRequest, Dto.CaseModel>
{
    public async Task<Dto.CaseModel> Handle(GetCaseByIdRequest request, CancellationToken cancellationToken)
    {
        DomainServices.CaseService.Contracts.Case? caseInstance = null;

        try
        {
            caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        }
        catch (CisNotFoundException)
        {
            // osetrena vyjimka - spoustime logiku na vytvoreni case z konsDB
            await _createCaseFromExternalSources.CreateCase(request.CaseId, cancellationToken);
            caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        }

        var model = await _converter.FromContract(caseInstance!);
        
        // case owner
        var userInstance = await _userService.GetUser(caseInstance!.CaseOwner.UserId, cancellationToken);
        model.CaseOwner = new Dto.CaseOwnerModel
        {
            Cpm = userInstance.UserInfo.Cpm,
            Icp = userInstance.UserInfo.Icp
        };

        return model;
    }

    private readonly Services.CreateCaseFromExternalSources.CreateCaseFromExternalSourcesService _createCaseFromExternalSources;
    private readonly CasesModelConverter _converter;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetCaseByIdHandler(
        Services.CreateCaseFromExternalSources.CreateCaseFromExternalSourcesService createCaseFromExternalSources,
        CasesModelConverter converter,
        DomainServices.UserService.Clients.IUserServiceClient userService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _createCaseFromExternalSources = createCaseFromExternalSources;
        _userService = userService;
        _converter = converter;
        _caseService = caseService;
    }
}

namespace NOBY.Api.Endpoints.Cases.GetCaseById;

internal sealed class GetCaseByIdHandler(
    Services.CreateCaseFromExternalSources.CreateCaseFromExternalSourcesService _createCaseFromExternalSources,
    CasesModelConverter _converter,
    DomainServices.UserService.Clients.v1.IUserServiceClient _userService,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<GetCaseByIdRequest, CasesSharedCaseModel>
{
    public async Task<CasesSharedCaseModel> Handle(GetCaseByIdRequest request, CancellationToken cancellationToken)
    {
        DomainServices.CaseService.Contracts.Case? caseInstance;

        try
        {
            caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        }
        catch (CisNotFoundException)
        {
            // osetrena vyjimka - spoustime logiku na vytvoreni case z konsDB
            await _createCaseFromExternalSources.CreateCase(request.CaseId);
            caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        }

        var model = await _converter.FromContract(caseInstance!);
        
        // case owner
        var userInstance = await _userService.GetUser(caseInstance!.CaseOwner.UserId, cancellationToken);
        model.CaseOwner = new CasesSharedCaseOwnerModel
        {
            Cpm = userInstance.UserInfo.Cpm,
            Icp = userInstance.UserInfo.Icp
        };

        return model;
    }
}

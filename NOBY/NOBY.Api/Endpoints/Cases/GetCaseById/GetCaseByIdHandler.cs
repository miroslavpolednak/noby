﻿namespace NOBY.Api.Endpoints.Cases.GetCaseById;

internal sealed class GetCaseByIdHandler(
    Services.CreateCaseFromExternalSources.CreateCaseFromExternalSourcesService _createCaseFromExternalSources,
    CasesModelConverter _converter,
    DomainServices.UserService.Clients.IUserServiceClient _userService,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<GetCaseByIdRequest, SharedDto.CaseModel>
{
    public async Task<SharedDto.CaseModel> Handle(GetCaseByIdRequest request, CancellationToken cancellationToken)
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
        model.CaseOwner = new SharedDto.CaseOwnerModel
        {
            Cpm = userInstance.UserInfo.Cpm,
            Icp = userInstance.UserInfo.Icp
        };

        return model;
    }
}

namespace FOMS.Api.Endpoints.Cases.GetById;

internal class GetByIdHandler
    : IRequestHandler<GetByIdRequest, Dto.CaseModel>
{
    public async Task<Dto.CaseModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var result = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.CaseId, cancellationToken));
        var model = await _converter.FromContract(result);
        
        // case owner
        var userInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.UserService.Contracts.User>(await _userService.GetUser(result.CaseOwner.UserId, cancellationToken));
        model.CaseOwner = new Dto.CaseOwnerModel
        {
            Cpm = userInstance.CPM,
            Icp = userInstance.ICP
        };

        return model;
    }

    private readonly CasesModelConverter _converter;
    private readonly DomainServices.UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetByIdHandler(
        CasesModelConverter converter,
        DomainServices.UserService.Abstraction.IUserServiceAbstraction userService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _userService = userService;
        _converter = converter;
        _caseService = caseService;
    }
}

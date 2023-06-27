using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Cases.GetCaseById;

internal sealed class GetCaseByIdHandler
    : IRequestHandler<GetCaseByIdRequest, Dto.CaseModel>
{
    public async Task<Dto.CaseModel> Handle(GetCaseByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // perm check
        if (result.CaseOwner.UserId != _currentUser.User!.Id && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException();
        }

        var model = await _converter.FromContract(result);
        
        // case owner
        var userInstance = await _userService.GetUser(result.CaseOwner.UserId, cancellationToken);
        model.CaseOwner = new Dto.CaseOwnerModel
        {
            Cpm = userInstance.UserInfo.Cpm,
            Icp = userInstance.UserInfo.Icp
        };

        return model;
    }

    private readonly ICurrentUserAccessor _currentUser;
    private readonly CasesModelConverter _converter;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetCaseByIdHandler(
        ICurrentUserAccessor currentUser,
        CasesModelConverter converter,
        DomainServices.UserService.Clients.IUserServiceClient userService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _currentUser = currentUser;
        _userService = userService;
        _converter = converter;
        _caseService = caseService;
    }
}

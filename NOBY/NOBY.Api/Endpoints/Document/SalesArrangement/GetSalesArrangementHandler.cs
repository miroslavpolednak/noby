using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document.SalesArrangement;

internal sealed class GetSalesArrangementHandler : IRequestHandler<GetSalesArrangementRequest, ReadOnlyMemory<byte>>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly DocumentManager _documentManager;

    public async Task<ReadOnlyMemory<byte>> Handle(GetSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSaService.GetCustomer(request.InputParameters.CustomerOnSaId!.Value, cancellationToken);
        var salesArrangementId = customerOnSa.SalesArrangementId;
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        var caseId = salesArrangement.CaseId; 
        
        var ownerUserId = (await _caseService.ValidateCaseId(caseId, true, cancellationToken)).OwnerUserId;
            
        if (_currentUser.User!.Id != ownerUserId && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException();
        }
        
        var generalDocumentRequest = new GeneralDocument.GetGeneralDocumentRequest
        {
            DocumentType = request.DocumentType,
            ForPreview = request.ForPreview,
            InputParameters = _documentManager.GetSalesArrangementInput(salesArrangementId, request.InputParameters.CustomerOnSaId)
        };

        return await _mediator.Send(generalDocumentRequest, cancellationToken);
    }

    public GetSalesArrangementHandler(
        IMediator mediator,
        ICurrentUserAccessor currentUser,
        ICustomerOnSAServiceClient customerOnSaService,
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService,
        DocumentManager documentManager)
    {
        _mediator = mediator;
        _currentUser = currentUser;
        _customerOnSaService = customerOnSaService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _documentManager = documentManager;
    }
}
using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.SharedDto;

namespace NOBY.Api.Endpoints.Document.CancelConfirmation;

internal sealed class GetSalesArrangementCancelConfirmationHandler : IRequestHandler<GetSalesArrangementCancelConfirmationRequest, ReadOnlyMemory<byte>>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly DocumentManager _documentManager;

    public async Task<ReadOnlyMemory<byte>> Handle(GetSalesArrangementCancelConfirmationRequest cancelConfirmationRequest, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSaService.GetCustomer(cancelConfirmationRequest.InputParameters.CustomerOnSaId!.Value, cancellationToken);
        var salesArrangementId = customerOnSa.SalesArrangementId;
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        var caseId = salesArrangement.CaseId; 
        
        var ownerUserId = (await _caseService.ValidateCaseId(caseId, true, cancellationToken)).OwnerUserId;
            
        if (_currentUser.User!.Id != ownerUserId && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException("Case owner check failed");
        }
        
        var generalDocumentRequest = new GeneralDocument.GetGeneralDocumentRequest
        {
            DocumentType = cancelConfirmationRequest.DocumentType,
            ForPreview = cancelConfirmationRequest.ForPreview,
            InputParameters = _documentManager.GetSalesArrangementInput(salesArrangementId, cancelConfirmationRequest.InputParameters.CustomerOnSaId)
        };

        return await _mediator.Send(generalDocumentRequest, cancellationToken);
    }

    public GetSalesArrangementCancelConfirmationHandler(
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
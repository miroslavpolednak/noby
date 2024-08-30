using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Document.SharedDto;

namespace NOBY.Api.Endpoints.Document.CancelConfirmation;

internal sealed class GetSalesArrangementCancelConfirmationHandler(
    IMediator _mediator,
    ICurrentUserAccessor _currentUser,
    ICustomerOnSAServiceClient _customerOnSaService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService,
    DocumentManager _documentManager) 
    : IRequestHandler<GetSalesArrangementCancelConfirmationRequest, ReadOnlyMemory<byte>>
{
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
}